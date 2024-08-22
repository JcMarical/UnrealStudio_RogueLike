using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GoodType
{
    ObtainableObject,
    Weapon
};

[SerializeField]
public class StoreRoomMgr : TInstance<StoreRoomMgr>
{
    [OdinSerialize] public StoreRoomData storeRoomData;
    [OdinSerialize] private List<ITradable> Goods = new();//储存当前出售的商品
    [OdinSerialize] public List<ITradable> AllObtianableObjects =new();//储存所有可出售的物品
    [OdinSerialize] public List<ITradable> AllWeapons = new();//储存所有可出售的武器
    [OdinSerialize] public List<List<ITradable>> ObtainableObjects_Leveled = new();//储存按稀有度分类的商品
    [OdinSerialize] public List<List<ITradable>> Weapons_Leveled = new();//储存按稀有度分类的武器
                    public List<Vector3> GoodsPos = new();//储存商品的位置，武器商品的位置在链表尾
                    public Dictionary<Vector3, ITradable> Shelve = new();//货架，储存商品的位置和商品的对应关系

    public GameObject GoodsTileMapContainer;
    public GameObject GoodsContainer;
    public GameObject WeaponContainer;
    public Tilemap SimpleGoodsTileMap;
    public Tilemap WeaponTileMap;

    [Header("参数设置")]
 
    [Header("Editor")]
    public Collection_Data SoldOutTest;
    public Collection_Data BoughtTest;
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
        
    }

    private void Init()
    {
        GoodsTileMapContainer = transform.GetChild(2).gameObject;
        SimpleGoodsTileMap = GoodsTileMapContainer.transform.GetChild(0).GetComponent<Tilemap>();
        WeaponTileMap = GoodsTileMapContainer.transform.GetChild(1).GetComponent<Tilemap>();

        SrotTheList();
        GetGoodsPos();
        InitShelve();
        InitGoodsList();
        ReListShelve();
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
        foreach (ITradable Obtain in AllObtianableObjects)
        {
            ObtainableObjects_Leveled[(int)(Obtain as ObtainableObjectData).Rarity].Add(Obtain);
        }
    }

    private void SortTheWeaponsByRarity()
    {
        foreach (ITradable Weapon in AllWeapons)
        {
            Weapons_Leveled[(int)(Weapon as WeaponData).Rarity].Add(Weapon);
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
        for (int i=0;i < storeRoomData.GoodsAmount-1 ; i++)
        {
            Goods.Add(RefreshGoodByPos(i));
        }
        Goods.Add(RefreshGoodByPos(GoodsPos.Count-1));
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
        GoodsPos.Add(weaponPos);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Commodity">商品信息</param>
    /// <param name="transform">商位的Transform</param>
    public bool BuyThings(ITradable Commodity,Transform transform)
    {
        if (PropBackPackUIMgr.Instance.CurrenetCoins >= Commodity.Price)
        {
            Commodity.BeBought(transform);
            PropBackPackUIMgr.Instance.CurrenetCoins -= Commodity.Price;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 卖物品
    /// </summary>
    public void SoldThings(ITradable Commodity)
    {
        Commodity.BeSoldOut();
        PropBackPackUIMgr.Instance.CurrenetCoins += Commodity.Price;    
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
                PropBackPackUIMgr.Instance.CurrenetCoins -= amount;
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
            PropBackPackUIMgr.Instance.CurrenetCoins += amount;
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
        if (Index != GoodsPos.Count - 1)
        {
            Good = AllObtianableObjects[GenerateUniqueRandomNumbers(0, AllObtianableObjects.Count - 1, 1)[0]];
            Shelve[GoodsPos[Index]] = Good;
        }
        else
        {
            Good = AllWeapons[GenerateUniqueRandomNumbers(0, AllWeapons.Count - 1, 1)[0]];
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
            if (count <= 3)
                GoodsContainer.transform.GetChild(count++).GetComponent<SpriteRenderer>().sprite = (Shelve[Pos] as ObtainableObjectData)?.Icon;

            else
                WeaponContainer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Shelve[Pos] as WeaponData)?.sprite;
        }
    }


    /// <summary>
    /// 随机刷新部分商品
    /// </summary>
    public void RefreshGoods()
    { 
        
    }

    /// <summary>
    /// 刷新所有商品
    /// </summary>
    public void RedreshAllGoods()
    { 
    
    }

    public ITradable GetRandomGoodByRarity(Rarities Rarity,GoodType GoodType)
    {
        switch (GoodType)
        {
            case GoodType.ObtainableObject:
                return ObtainableObjects_Leveled[(int)Rarity][GetRandomNumber(0, ObtainableObjects_Leveled[(int)Rarity].Count - 1)];

            case GoodType.Weapon:
                return Weapons_Leveled[(int)Rarity][GetRandomNumber(0, Weapons_Leveled[(int)Rarity].Count - 1)];

            default:
                return null;
        }
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
}

public interface ITradable
{
    public int Price{ get; set; }
    public void BeBought(Transform transform);
    public void BeSoldOut();

    public GoodType GoodType { get; }
}

