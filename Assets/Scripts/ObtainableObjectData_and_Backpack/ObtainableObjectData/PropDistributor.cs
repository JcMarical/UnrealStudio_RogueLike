using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDistributor : TInstance<PropDistributor>
{
    public List<Collection_Data> AllCollections = new List<Collection_Data>();//所有藏品的备份，不按等级排序
    public List<Prop_Data> AllProps = new List<Prop_Data>();//所有道具的备份，不按等级排序

    public List<List<Collection_Data>> collection_Datas = new();
    public List<List<Prop_Data>> prop_Datas = new();

    public Collection_Data DefualtCollection;

    [Header("Editor")]
    public int CollectionLevel = 1;
    public int PropLevel = 1;
    public Enemy TestEnemy;


    protected override void Awake()
    {
        base.Awake();
        collection_Datas = InitCollectionLists(AllCollections);
        prop_Datas = InitPropLists(AllProps);
    }

    /// <summary>
    /// 将所有藏品按照等级分开
    /// </summary>
    /// <param name="allDatas">未排序链表</param>
    private List<List<Collection_Data>> InitCollectionLists(List<Collection_Data> allDatas)
    {
        List<List<Collection_Data>> LeveledList = new();
        int maxLevel = 0;
        foreach (Collection_Data data in allDatas)
        {
            if ((int)data.Rarity > maxLevel)
                maxLevel = (int)data.Rarity;
        }

        for (int i = 0; i <= maxLevel; i++)
        {
            LeveledList.Add(new List<Collection_Data>());
        }

        foreach (Collection_Data data in allDatas)
        {
            LeveledList[(int)data.Rarity].Add(data);
        }

        return LeveledList;
    }

    /// <summary>
    /// 将所有道具按照等级分开
    /// </summary>
    /// <param name="allDatas">未排序链表</param>
    private List<List<Prop_Data>> InitPropLists(List<Prop_Data> allDatas)
    {
        List<List<Prop_Data>> LeveledList = new();
        int maxLevel = 0;
        foreach (Prop_Data data in allDatas)
        {
            if ((int)data.Rarity > maxLevel)
                maxLevel = (int)data.Rarity;
        }

        for (int i = 0; i <= maxLevel; i++)
        {
            LeveledList.Add(new List<Prop_Data>());
        }

        foreach (Prop_Data data in allDatas)
        {
            LeveledList[(int)data.Rarity].Add(data);
        }

        return LeveledList;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IfthisPossible(float percent)
    {
        int seed = DateTime.Now.GetHashCode();
        System.Random newrandom = new System.Random(seed);
        double chance = newrandom.NextDouble();
        return chance <= percent;
    }

    /// <summary>
    /// 掉落指定等级的藏品   
    /// </summary>
    /// <param name="level">等级</param>
    public Collection_Data DistributeRandomCollectionbyLevel(int level)
    {
        if (collection_Datas[level].Count -1 >= level)
        {
            Collection_Data result;
            int randomIndex = UnityEngine.Random.Range(0, collection_Datas[level].Count);
            result = collection_Datas[level][randomIndex];
            collection_Datas[level].RemoveAt(randomIndex);
            return result;
        }
        return DefualtCollection;
    }

    /// <summary>
    /// 掉落指定等级的道具
    /// </summary>
    /// <param name="level">等级</param>
    public Prop_Data DistributeRandomPropbyLevel(int level)
    {
        if (prop_Datas.Count-1 >= level)
        {
            Prop_Data result;
            int randomIndex = UnityEngine.Random.Range(0, prop_Datas[level].Count);
            result = prop_Datas[level][randomIndex];
            prop_Datas[level].RemoveAt(randomIndex);
            return result;
        }
        return null;
    }

    /// <summary>
    /// 掉落骰子
    /// </summary>
    public void DistributeDice(int Amount)
    { 
    
    }

    /// <summary>
    /// 掉落金币
    /// </summary>
    public void DistributeCoin(int Amount)
    { 
    
    }

    public void WhenEnemyDies(Enemy target)
    {
        float chance_Drop = 0;
        switch (target.enemyQuality)
        {
            case Enemy.EnemyQuality.normal:
                chance_Drop = 0.1f;
                break;
                
            case Enemy.EnemyQuality.elite:
                chance_Drop = 0.3f;
                break;

            default:
                return;
        }

        if (IfthisPossible(chance_Drop))
        { 
            int randomNumber = UnityEngine.Random.Range(0, 100);
            ObtainableObjectData objects;
            if (randomNumber < 70)
            {
                if (DateTime.Now.GetHashCode() % 2 == 0)
                {
                    objects = DistributeRandomCollectionbyLevel(1);
                    Debug.Log("掉落1级藏品");
                }
                else
                {
                    objects = DistributeRandomPropbyLevel(1);
                    Debug.Log("掉落1级道具");
                }
                if(objects) StartCoroutine(objects.OnDistributed(target.transform.position, GameObject.FindGameObjectWithTag("Player")));
            }

            else if (randomNumber < 80)
            {
                if (DateTime.Now.GetHashCode() % 2 == 0)
                {
                    objects = DistributeRandomCollectionbyLevel(2);
                    Debug.Log("掉落2级藏品");
                }
                else
                {
                    objects = DistributeRandomPropbyLevel(2);
                    Debug.Log("掉落2级道具");
                }
                if(objects)StartCoroutine(objects.OnDistributed(target.transform.position, GameObject.FindGameObjectWithTag("Player")));
            }

            else
            {
                DistributeDice(1);
            }
        }
    }
}
