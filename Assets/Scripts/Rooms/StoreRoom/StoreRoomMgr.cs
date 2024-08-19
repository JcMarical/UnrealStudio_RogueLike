using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class StoreRoomMgr : TInstance<StoreRoomMgr>
{
    public static StoreRoomData storeRoomData;
    [OdinSerialize] private List<ITradable> Goods = new(); 
    public List<ITradable> AllObtianableObjects =new();
    [OdinSerialize] public List<ITradable> AllWeapons = new();

    [OdinSerialize]public Dictionary<int,string> OdinTest = new();
    [OdinSerialize] private ITradable test;
 
    [Header("Editor")]
    public Collection_Data SoldOutTest;
    public Collection_Data BoughtTest;
    public int StoreTestAmount;
    public int TakeOutTestAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Commodity">商品信息</param>
    /// <param name="transform">商位的Transform</param>
    public void BuyThings(ITradable Commodity,Transform transform)
    {
        if (PropBackPackUIMgr.Instance.CurrenetCoins >= Commodity.Price)
        {
            Commodity.BeBought(transform);
            PropBackPackUIMgr.Instance.CurrenetCoins -= Commodity.Price;
        }
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
    public void Storage(int amount)
    {
        if (amount <= PropBackPackUIMgr.Instance.CurrenetCoins)
        {
            if (amount + PropBackPackUIMgr.Instance.StoredCoins.Amount <= storeRoomData.MoneyStoreMaximums)
            {
                PropBackPackUIMgr.Instance.CurrenetCoins -= amount;
                PropBackPackUIMgr.Instance.StoredCoins.GainResource(amount);
            }
            else
            { 
                amount = storeRoomData.MoneyStoreMaximums - PropBackPackUIMgr.Instance.StoredCoins.Amount;
                //TODO:提示存储上限
                Storage(amount);
            }
        }
        else
        {
            Debug.Log("金币不足");
            //TODO:提示金币不足
        }

        if (PropBackPackUIMgr.Instance.StoredCoins.Amount >= 10)
        {
            storeRoomData.GoodsAmount = 4;
            if (PropBackPackUIMgr.Instance.StoredCoins.Amount >=30 )
            {
                storeRoomData.GoodsAmount = 5;
                if (PropBackPackUIMgr.Instance.StoredCoins.Amount >= 60)
                { 
                    storeRoomData.ifSpecialGoods = true;
                }
            }
        }
    }

    /// <summary>
    /// 取钱
    /// </summary>
    public void TakeOut(int amount)
    {
        if (PropBackPackUIMgr.Instance.StoredCoins.Amount >= amount && PropBackPackUIMgr.Instance.CurrenetCoins >= PropBackPackUIMgr.Instance.StoredCoins.TakeOutCost)
        { 
            PropBackPackUIMgr.Instance.CurrenetCoins += amount;
            PropBackPackUIMgr.Instance.StoredCoins.CostResource(amount);
            PropBackPackUIMgr.Instance.StoredCoins.TakeOutCost++;
        }
    }

    /// <summary>
    /// 刷新部分商品
    /// </summary>
    public void RefreshGoods()
    {
        var RandomIndexs = GenerateUniqueRandomNumbers(0,Goods.Count,2);
        foreach (var Index in RandomIndexs)
        {
            Goods[Index] = GetRandomGood(Index);
        }
    }

    /// <summary>
    /// 刷新所有商品
    /// </summary>
    public void RefreshAllGoods()
    {
        for(int i=0; i < storeRoomData.GoodsAmount; i++)
        {
            Goods[i] = GetRandomGood(i);
        }
    }

    /// <summary>
    /// 返回随机商品
    /// </summary>
    private ITradable GetRandomGood(int Index)
    {
        int RandomIndex = 0;
        if (Index != 3)
        { 
            RandomIndex = Random.Range(0, AllObtianableObjects.Count);
            return AllObtianableObjects[RandomIndex];
        }
        return AllWeapons[Random.Range(0,AllWeapons.Count)];
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
}

public interface ITradable
{
    public int Price{ get; set; }
    public void BeBought(Transform transform);
    public void BeSoldOut();
}

