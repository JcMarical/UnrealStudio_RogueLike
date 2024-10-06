using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

public struct RarityandProbabilityofStorePerLayer
{
    public Rarities minRarity;//当前层能获得的物品的最低稀有度
    public Rarities maxRarity;//当前层能获得的物品的最高稀有度
    public Dictionary<Rarities, float> Probability;//当前层每个稀有度的概率 key：稀有度 value：概率

    public RarityandProbabilityofStorePerLayer(Rarities minRarity,Rarities maxRarity, Dictionary<Rarities, float> Probability)
    {
        this.minRarity = minRarity;
        this.maxRarity = maxRarity;
        this.Probability = Probability;
    }

    public float GetChanceByRarity(Rarities Rarity)
    {
        if ((int)Rarity <= (int)maxRarity)
        {
            return Probability[Rarity];
        }
        else return -1;
    }
};

public struct RarityandProbabilityOfObectRoomPerLayer
{
    public Rarities minRarity;//当前层能获得的物品的最低稀有度
    public Rarities maxRarity;//当前层能获得的物品的最高稀有度
    public Dictionary<Rarities, float> Probability;//当前层每个稀有度的概率 key：稀有度 value：概率

    public RarityandProbabilityOfObectRoomPerLayer(Rarities minRarity, Rarities maxRarity, Dictionary<Rarities, float> Probability)
    {
        this.minRarity = minRarity;
        this.maxRarity = maxRarity;
        this.Probability = Probability;
    }

    public float GetChanceByRarity(Rarities Rarity)
    {
        if ((int)Rarity <= (int)maxRarity)
        {
            return Probability[Rarity];
        }
        else return -1;
    }
}

public class GameManager : TInstance<GameManager>
{
    #region"各种概率"
    public RarityandProbabilityofStorePerLayer[] RAP_Store;//商店每层的稀有度和概率，缩写为RAP，注意：RAP[0]是占位符，不应被使用！！！
    public RarityandProbabilityOfObectRoomPerLayer[] RAP_ObjectRoom;//物品房每层的稀有度和概率，缩写为RAP，注意：RAP[0]是占位符，不应被使用！！！
    #endregion
    public int CurrentLayer = 1;

    #region"Mgr初始化"
    private string MgrPrefabPath = "Prefabs/MgrPrefab/";
    private List<Type> MgrType = new List<Type>()
    {
        typeof(SS_Mgr),
        typeof(PropDistributor)
    };
    #endregion

    [SerializeField] private float unease;
    [Tooltip("不安值")] public float Unease
    {
        get => unease;
        set => unease = value > 0 ? value : 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        LoadAllMgr();
    }

    private void LoadAllMgr()
    {
        GameObject MgrContainer = new GameObject("MgrContainer");
        //DontDestroyOnLoad(MgrContainer);
        foreach (var mgr in MgrType)
        {
            string PrefabPath = "Prefabs/MgrPrefab/" + mgr.Name;
            var obj = Resources.Load(PrefabPath);
            if (obj)
            {
                GameObject go = Instantiate(obj) as GameObject;
                go.transform.SetParent(MgrContainer.transform);
            }
            else
            {
                Debug.Log("Mgr预制体加载失败，检查预制体名称");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 返回当前层商店出售物品的稀有度和概率
    /// </summary>
    /// <returns></returns>
    public RarityandProbabilityofStorePerLayer GetCurrentRAP_Store()
    { 
        return RAP_Store[CurrentLayer];
    }

    /// <summary>
    /// 返回当前层物品房出现物品的稀有度和概率
    /// </summary>
    /// <returns></returns>
    public RarityandProbabilityOfObectRoomPerLayer GetCurrentRAP_ObjectRoom()
    { 
        return RAP_ObjectRoom[CurrentLayer];
    }
}
