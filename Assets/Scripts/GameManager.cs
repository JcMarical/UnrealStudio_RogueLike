using System.Collections;
using System.Collections.Generic;
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
    public RarityandProbabilityofStorePerLayer[] RAP_Store;//商店每层的稀有度和概率，缩写为RAP，注意：RAP[0]是占位符，不应被使用！！！
    public RarityandProbabilityOfObectRoomPerLayer[] RAP_ObjectRoom;//物品房每层的稀有度和概率，缩写为RAP，注意：RAP[0]是占位符，不应被使用！！！
    public int CurrentLayer = 1;

    [SerializeField] private float unease;
    [Tooltip("不安值")] public float Unease
    {
        get => unease;
        set => unease = value > 0 ? value : 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
