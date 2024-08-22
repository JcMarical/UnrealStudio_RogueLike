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
    /// <param name="Commodity">��Ʒ��Ϣ</param>
    /// <param name="transform">��λ��Transform</param>
    public void BuyThings(ITradable Commodity,Transform transform)
    {
        if (PropBackPackUIMgr.Instance.CurrenetCoins >= Commodity.Price)
        {
            Commodity.BeBought(transform);
            PropBackPackUIMgr.Instance.CurrenetCoins -= Commodity.Price;
        }
    }

    /// <summary>
    /// ����Ʒ
    /// </summary>
    public void SoldThings(ITradable Commodity)
    {
        Commodity.BeSoldOut();
        PropBackPackUIMgr.Instance.CurrenetCoins += Commodity.Price;    
    }

    /// <summary>
    /// ��Ǯ
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
                //TODO:��ʾ�洢����
                Storage(amount);
            }
        }
        else
        {
            Debug.Log("��Ҳ���");
            //TODO:��ʾ��Ҳ���
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
    /// ȡǮ
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
    /// ˢ�²�����Ʒ
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
    /// ˢ��������Ʒ
    /// </summary>
    public void RefreshAllGoods()
    {
        for(int i=0; i < storeRoomData.GoodsAmount; i++)
        {
            Goods[i] = GetRandomGood(i);
        }
    }

    /// <summary>
    /// ���������Ʒ
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
    /// ��ָ����Χ������ n �����ظ�������
    /// </summary>
    /// <param name="min">��Χ����Сֵ��������</param>
    /// <param name="max">��Χ�����ֵ��������</param>
    /// <param name="count">Ҫ���ɵ���������</param>
    /// <returns>���ɵĲ��ظ������б�</returns>
    private List<int> GenerateUniqueRandomNumbers(int min, int max, int count)
    {
        if (count > (max - min + 1))
        {
            Debug.LogError("��Χ�ڵ����������������������������Ĳ��ظ�����");
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

