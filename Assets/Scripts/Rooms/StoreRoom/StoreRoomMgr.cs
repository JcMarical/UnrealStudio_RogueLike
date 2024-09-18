using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum GoodType
{
    ObtainableObject,
    Weapon
};

//当需要更新商店商品时，要维护的数据结构为：Goods链表，Shelve货架字典

[SerializeField]
public class StoreRoomMgr : TInstance<StoreRoomMgr>
{
    [SerializeField] private string ObtainableObjectsDataPath = "Datas/ObtainableObjects";
    [SerializeField] private string WeaponDataPath = "Datas/Weapon";


    public StoreRoomData storeRoomData;
    [OdinSerialize] private List<ITradable> Goods = new();//储存当前出售的商品
    [OdinSerialize] public List<ITradable> AllObtianableObjects =new();//储存所有可出售的物品
    [OdinSerialize] public List<ITradable> AllWeapons = new();//储存所有可出售的武器
    [OdinSerialize] public List<List<ITradable>> ObtainableObjects_Leveled = new();//储存按稀有度分类的商品
    [OdinSerialize] public List<List<ITradable>> Weapons_Leveled = new();//储存按稀有度分类的武器
    [OdinSerialize] public List<Vector3> GoodsPos = new();//储存商品的位置，武器商品的位置在链表头
                    public Dictionary<Vector3, ITradable> Shelve = new();//货架，储存商品的位置和商品的对应关系

    public GameObject GoodsTileMapContainer;
    public GameObject GoodsContainer;
    public GameObject WeaponContainer;
    public Tilemap SimpleGoodsTileMap;
    public Tilemap WeaponTileMap;

    [Header("UI引导")]
    public Text Buy_Direction;//购买引导的UI
    public float Buy_Direction_Offset;//购买引导的UI偏移量
    public  float Buy_Distance_Limit;//购买引导的UI距离限制
 
    [Header("Editor")]
    public int StoreTestAmount;
    public int TakeOutTestAmount;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetClosetGood(GameObject.FindGameObjectWithTag("Player"), Buy_Distance_Limit) != null)
        {
            Buy_Direction.gameObject.SetActive(true);
            Buy_Direction.rectTransform.anchoredPosition = GoodsPos[Goods.IndexOf(GetClosetGood(GameObject.FindGameObjectWithTag("Player"), Buy_Distance_Limit))] + new Vector3(0,1,0) * Buy_Direction_Offset;
        }
        else
        {
            Buy_Direction.gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        GoodsTileMapContainer = transform.GetChild(2).gameObject;
        SimpleGoodsTileMap = GoodsTileMapContainer.transform.GetChild(0).GetComponent<Tilemap>();
        WeaponTileMap = GoodsTileMapContainer.transform.GetChild(1).GetComponent<Tilemap>();

        GetAllITradable();
        SrotTheList();
        GetGoodsPos();
        InitShelve();
        InitGoodsList();
        ReFreshAllGoods();
    }

    #region 数据初始化

    private void GetAllITradable()
    {
        var Obtain = Resources.LoadAll(ObtainableObjectsDataPath);
        foreach (var obj in Obtain)
        {
            Debug.Log(obj.name);
            var g = obj as ITradable;
            if (g != null)
            {
                bool canSold = (g as ObtainableObjectData).SoldInStore;
                if (!AllObtianableObjects.Contains(g) && canSold)
                {
                    AllObtianableObjects.Add(g);
                }
            }
        }

        var Weapon = Resources.LoadAll(WeaponDataPath);
        foreach (var obj in Weapon)
        {
            Debug.Log(obj.name);
            var g = obj as ITradable;
            if (g != null && !AllWeapons.Contains(g))
            {
                AllWeapons.Add(g);
            }
        }
    }

    /// <summary>
    /// 将所有商品按稀有度分类
    /// </summary>
    private void SrotTheList()
    {
        SortTheObtainableObjectsByRarity();
        SortTheWeaponsByRarity();
    }

    private void SortTheObtainableObjectsByRarity()
    {
        for (int i=0;i < (int)Rarities.UR; i++)
        {
            ObtainableObjects_Leveled.Add(new());
        }
        
        foreach (ITradable Obtain in AllObtianableObjects)
        {
            if(!PropBackPackUIMgr.Instance.CollectionDatas.Contains(Obtain as Collection_Data)) 
                ObtainableObjects_Leveled[(int)(Obtain as ObtainableObjectData).Rarity].Add(Obtain);
        }
    }

    private void SortTheWeaponsByRarity()
    {
        for (int i = 0; i < (int)Rarities.UR; i++)
        {
            Weapons_Leveled.Add(new());
        }

        foreach (ITradable Weapon in AllWeapons)
        {
            Weapons_Leveled[(int)(Weapon as WeaponData).rarity].Add(Weapon);
        }
    }

    private void InitShelve()
    {
        Shelve.Clear();
        for (int i=0;i < GoodsPos.Count ;i++ )
        {
            Shelve.Add(GoodsPos[i],null);
        }
    }

    private void InitGoodsList()
    {
        //for (int i=0;i < storeRoomData.GoodsAmount-1 ; i++)
        //{
        //    Goods.Add(RefreshGoodByPos(i));
        //}
        for (int i=0;i<5 ;i++)
        {
            Goods.Add(null);
        }
    }

    private void GetGoodsPos()
    {
        BoundsInt bounds = SimpleGoodsTileMap.cellBounds;

        // 遍历边界内的所有单元格
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = SimpleGoodsTileMap.GetTile(tilePosition);

                if (tile != null)
                {
                    GoodsPos.Add(SimpleGoodsTileMap.CellToWorld(tilePosition) + 0.5f * SimpleGoodsTileMap.cellSize);
                }
            }
        }

        bounds = WeaponTileMap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = WeaponTileMap.GetTile(tilePosition);

                if (tile != null)
                {
                    GoodsPos.Add(WeaponTileMap.CellToWorld(tilePosition) + 0.5f * WeaponTileMap.cellSize);
                }
            }
        }

        Vector2 weaponPos = (GoodsPos[GoodsPos.Count - 1] + GoodsPos[GoodsPos.Count - 2]) / 2;
        GoodsPos.RemoveAt(GoodsPos.Count - 1);
        GoodsPos.RemoveAt(GoodsPos.Count - 1);
        GoodsPos.Insert(0, weaponPos);
    }

    #endregion

    /// <summary>
    /// 买东西
    /// </summary>
    /// <param name="Commodity">商品信息</param>
    /// <param name="transform">商位的Transform</param>
    public bool BuyThings(ITradable Commodity)
    {
        int Index = Goods.IndexOf(Commodity);
        Vector3 pos = GoodsPos[Index];
        if (PropBackPackUIMgr.Instance.CurrenetCoins >= Commodity.Price)
        {
            Commodity.BeBought(pos);
            PropBackPackUIMgr.Instance.ConsumeCoin(Commodity.Price);
            if(Commodity as Collection_Data) ObtainableObjects_Leveled[(int)(Commodity as Collection_Data).Rarity].Remove(Commodity);
            Goods[Index] = null;
            Shelve[pos] = null;
            ReListShelve();
            return true;
        }
        else
        {
            Debug.Log("没钱了");
        }
        return false;
    }

    /// <summary>
    /// 卖物品
    /// </summary>
    public void SoldThings(ITradable Commodity)
    {
        if ((Commodity as ObtainableObjectData)?.ID != 19 || (Commodity as WeaponData))
        {
            Commodity.BeSoldOut();
            if (Commodity as Collection_Data) ObtainableObjects_Leveled[(int)(Commodity as Collection_Data).Rarity].Add(Commodity);
            PropBackPackUIMgr.Instance.GainCoin(Commodity.Price);
        }
    }

    /// <summary>
    /// 存钱
    /// </summary>
    public bool Storage(int amount)
    {
        bool res = false;
        if (amount <= PropBackPackUIMgr.Instance.CurrenetCoins)
        {
            if (amount + PropBackPackUIMgr.Instance.StoredCoins.Amount <= storeRoomData.MoneyStoreMaximums)
            {
                PropBackPackUIMgr.Instance.ConsumeCoin(amount);
                PropBackPackUIMgr.Instance.StoredCoins.GainResource(amount);
                res = true;
            }
            else
            { 
                amount = storeRoomData.MoneyStoreMaximums - PropBackPackUIMgr.Instance.StoredCoins.Amount;
                //TODO:提示存储上限
                Storage(amount);
                res = true;
            }
        }
        else
        {
            Debug.Log("金币不足");
            res = false;
            //TODO:提示金币不足
        }

        if (PropBackPackUIMgr.Instance.StoredCoins.Amount >= 100)
        {
            storeRoomData.GoodsAmount = 4;
            if (PropBackPackUIMgr.Instance.StoredCoins.Amount >=300)
            {
                storeRoomData.GoodsAmount = 5;
                if (PropBackPackUIMgr.Instance.StoredCoins.Amount >= 600)
                { 
                    storeRoomData.ifSpecialGoods = true;
                }
            }
        }

        return res;
    }

    /// <summary>
    /// 取钱
    /// </summary>
    public bool TakeOut(int amount)
    {
        if (PropBackPackUIMgr.Instance.StoredCoins.Amount >= amount && PropBackPackUIMgr.Instance.CurrenetCoins >= PropBackPackUIMgr.Instance.StoredCoins.TakeOutCost)
        { 
            PropBackPackUIMgr.Instance.GainDice(amount);
            PropBackPackUIMgr.Instance.StoredCoins.CostResource(amount);
            PropBackPackUIMgr.Instance.StoredCoins.TakeOutCost++;
            return true;
        }
        return false;
    }


    /// <summary>
    /// 为GoodsPos链表中指定索引处刷新商品
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    private ITradable RefreshGoodByPos(int Index)
    {
        ITradable Good = null;
        if (Index != 0)
        {
            Good = Instantiate(AllObtianableObjects[GenerateUniqueRandomNumbers(0, AllObtianableObjects.Count - 1, 1)[0]] as ScriptableObject) as ITradable;
            Shelve[GoodsPos[Index]] = Good;
        }
        else
        {
            Good = Instantiate(AllWeapons[GenerateUniqueRandomNumbers(0, AllWeapons.Count - 1, 1)[0]] as ScriptableObject) as ITradable;
            Shelve[GoodsPos[Index]] = Good;
        }

        return Good;
    }

    /// <summary>
    /// 刷新商品UI
    /// </summary>
    public void ReListShelve()
    {
        int count = 0;
        foreach (Vector3 Pos in GoodsPos)
        {
            if (count!=0)
                GoodsContainer.transform.GetChild(count++ -1).GetComponent<SpriteRenderer>().sprite = (Shelve[Pos] as ObtainableObjectData)?.Icon;

            else
                WeaponContainer.transform.GetChild(count++).GetComponent<SpriteRenderer>().sprite = (Shelve[Pos] as WeaponData)?.sprite;
        }
    }


    /// <summary>
    /// 随机刷新两件商品
    /// </summary>
    [Button("随机刷新两件商品")]
    public void RefreshGoods()
    {
        RarityandProbabilityofStorePerLayer RAP = GameManager.Instance.GetCurrentRAP_Store();
        List<int> RandomPosIndex = GenerateUniqueRandomNumbers(0,storeRoomData.GoodsAmount-1,2);//获取两个要刷新商品的位置
        foreach (int PosIndex in RandomPosIndex)
        {
            GoodType type = PosIndex == 0 ? GoodType.Weapon: GoodType.ObtainableObject;
            ReplaceGood(PosIndex,GetGoodsWithRarityLimit(RAP,type));
        }
    }

    /// <summary>
    /// 刷新所有商品
    /// </summary>
    [Button("刷新所有商品")]
    public void ReFreshAllGoods()
    {
        RarityandProbabilityofStorePerLayer RAP = GameManager.Instance.GetCurrentRAP_Store();
        for (int PosIndex = 0;PosIndex < storeRoomData.GoodsAmount;PosIndex++)
        {
            GoodType type = PosIndex == 0 ? GoodType.Weapon : GoodType.ObtainableObject;
            ReplaceGood(PosIndex, GetGoodsWithRarityLimit(RAP, type));
        }
    }

    /// <summary>
    /// 替换商品,会维护Goods链表和Shelve字典
    /// </summary>
    /// <param name="Original">原来的商品</param>
    /// <param name="New">现在的商品</param>
    private void ReplaceGood(int OriginalIndex,ITradable New)
    {
        ITradable Original = Goods[OriginalIndex];
        Goods.RemoveAt(OriginalIndex);
        Goods.Insert(OriginalIndex, New);
        Shelve[GoodsPos[OriginalIndex]] = New;
        ReListShelve();
        Destroy(Original as Object);
    }

    /// <summary>
    /// 在不同的概率要求下获取随机商品
    /// </summary>
    /// <param name="RAP">概率要求结构体</param>
    /// <param name="GoodType">商品类型</param>
    /// <returns></returns>
    private ITradable GetGoodsWithRarityLimit(RarityandProbabilityofStorePerLayer RAP,GoodType GoodType)
    {
        int RandomNumber = GetRandomNumber(1, 100);
        Rarities resRarity = RAP.minRarity;
        while (RAP.Probability[resRarity] < RandomNumber)
        {
            resRarity++;
        }

        switch (GoodType)
        {
            case GoodType.ObtainableObject:
                while (ObtainableObjects_Leveled[(int)resRarity].Count == 0)
                {
                    resRarity--;
                }
            break;

            case GoodType.Weapon:
                while (Weapons_Leveled[(int)resRarity].Count == 0)
                {
                    resRarity--;
                }
            break;
        }
        return GetRandomGoodByRarity(resRarity, GoodType);
    }

    /// <summary>
    /// 随机获取到指定稀有度的商品
    /// </summary>
    /// <param name="Rarity">稀有度</param>
    /// <param name="GoodType">商品类型</param>
    /// <returns></returns>
    public ITradable GetRandomGoodByRarity(Rarities Rarity,GoodType GoodType)
    {
        ITradable res;
        switch (GoodType)
        {
            case GoodType.ObtainableObject:
                res = (Instantiate(ObtainableObjects_Leveled[(int)Rarity][GetRandomNumber(0, ObtainableObjects_Leveled[(int)Rarity].Count - 1)] as ScriptableObject) as ITradable);
                if (res as Collection_Data)
                {
                    ObtainableObjects_Leveled[(int)(res as Collection_Data).Rarity].Remove(ObtainableObjects_Leveled[(int)(res as Collection_Data).Rarity].Find(x => (x as Collection_Data).ID == (res as Collection_Data).ID));
                }
                break;

            case GoodType.Weapon:
                res = (Instantiate(Weapons_Leveled[(int)Rarity][GetRandomNumber(0, Weapons_Leveled[(int)Rarity].Count - 1)] as ScriptableObject) as ITradable);
                break;

            default:
                return null;
        }

        return res;
    }

    /// <summary>
    /// 在指定范围内生成 n 个不重复的整数
    /// </summary>
    /// <param name="min">范围的最小值（包含）</param>
    /// <param name="max">范围的最大值（包含）</param>
    /// <param name="count">要生成的整数数量</param>
    /// <returns>生成的不重复整数列表</returns>
    private List<int> GenerateUniqueRandomNumbers(int min, int max, int count)
    {
        if (max < min)
        {
            Debug.LogError("最大值比最小值小，最大值是" + max +"最小值是" + min);
        }

        if (count > (max - min + 1))
        {
            Debug.LogError("范围内的数字数量不足以生成所需数量的不重复整数");
            return null;
        }

        HashSet<int> uniqueNumbers = new HashSet<int>();
        while (uniqueNumbers.Count < count)
        {
            int number = Random.Range(min, max + 1);
            uniqueNumbers.Add(number);
        }

        return new List<int>(uniqueNumbers);
    }

    /// <summary>
    /// 在指定范围内生成一个随机整数
    /// </summary>
    /// <param name="min">最小值（包含）</param>
    /// <param name="max">最大值（包含）</param>
    /// <returns></returns>
    private int GetRandomNumber(int min,int max)
    {
        return GenerateUniqueRandomNumbers(min,max,1)[0];
    }

    /// <summary>
    /// 获取到离顾客最近的商品
    /// </summary>
    /// <param name="Customer">谁在买东西</param>
    /// <param name="DistanceLimit">距离限制，超过此距离将返回空</param>
    /// <returns></returns>
    public ITradable GetClosetGood(GameObject Customer,float DistanceLimit)
    {
        Vector3 Cloest = GoodsPos[0];
        foreach (Vector3 pos in GoodsPos)
        {
            if (Vector3.Distance(pos,Customer.transform.position) < Vector3.Distance(Cloest,Customer.transform.position))
            { 
                Cloest = pos;
            }
        }

        if (Vector3.Distance(Cloest, Customer.transform.position) <= DistanceLimit) return Shelve[Cloest];

        return null;
    }

    private void RePleaceComponentInList<T>(List<T> targetList,T Original,T New)
    {
        int Index = targetList.IndexOf(Original);
        targetList.Insert(Index, New);
        targetList.Remove(Original);
    }
}

public interface ITradable
{
    public int Price{ get; set; }
    public void BeBought(Vector3 startPos);
    public abstract void BeSoldOut();

    public GoodType GoodType { get; set; }
}



