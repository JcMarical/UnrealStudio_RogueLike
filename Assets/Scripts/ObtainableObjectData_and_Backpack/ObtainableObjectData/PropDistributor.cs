using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PropDistributor : TInstance<PropDistributor>
{
    public List<Collection_Data> AllCollections = new List<Collection_Data>();//���в�Ʒ�ı��ݣ������ȼ�����
    public List<Prop_Data> AllProps = new List<Prop_Data>();//���е��ߵı��ݣ������ȼ�����
    public List<GameObject> AllWeapons=new List<GameObject>();//�����������ݣ������ȼ�����

    public List<List<Collection_Data>> collection_Datas = new();
    public List<List<Prop_Data>> prop_Datas = new();
    public List<List<GameObject>> weapons=new();

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
        weapons= InitWeaponLists(AllWeapons);
    }

    /// <summary>
    /// �����в�Ʒ���յȼ��ֿ�
    /// </summary>
    /// <param name="allDatas">δ��������</param>
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
    /// �����е��߰��յȼ��ֿ�
    /// </summary>
    /// <param name="allDatas">δ��������</param>
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
    /// <summary>
    /// �������������ȼ��ֿ�
    /// </summary>
    /// <param name="allDatas"></param>
    /// <returns></returns>
    private List<List<GameObject>> InitWeaponLists(List<GameObject> allDatas){
        List<List<GameObject>> LeveledList = new();
        int maxLevel = 0;
        foreach (GameObject data in allDatas)
        {
            if ((int)data.GetComponent<Weapon>().weaponData.rarity > maxLevel)
                maxLevel = (int)data.GetComponent<Weapon>().weaponData.rarity;
        }

        for (int i = 0; i <= maxLevel; i++)
        {
            LeveledList.Add(new List<GameObject>());
        }

        foreach (GameObject data in allDatas)
        {
            LeveledList[(int)data.GetComponent<Weapon>().weaponData.rarity].Add(data);
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
    /// ��ȡָ���ȼ�������
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public GameObject DistributeRandomWeaponbyLevel(int level){
        if (weapons.Count -1 >= level)
        {
            GameObject result;
            int randomIndex = UnityEngine.Random.Range(0, weapons[level].Count);
            result = weapons[level][randomIndex];
            return result;
        }
        else{
            GameObject result;
            int randomIndex = UnityEngine.Random.Range(0, weapons[weapons.Count-1].Count);
            result = weapons[weapons.Count-1][randomIndex];
            return result;
        }
    }
    /// <summary>
    /// ����ָ���ȼ��Ĳ�Ʒ   
    /// </summary>
    /// <param name="level">�ȼ�</param>
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
    /// ʵ�����������������Ķ���Ч��
    /// </summary>
    /// <param name="startPos">����Ч������Ʒ������ʼ��</param>
    /// <param name="targetPos">����Ч������Ʒ�����յ�</param>
    /// <param name="targetProp">����Ԥ����</param>
    public void DistributeProp(Vector3 startPos,Vector3 target,GameObject weapon)
    {
        GameObject w=Instantiate(weapon, startPos, Quaternion.identity);
        w.transform.DOMove(target,0.8f);
    }
    /// <summary>
    /// ʵ�����������������Ķ���Ч��
    /// </summary>
    /// <param name="startPos">����Ч������Ʒ������ʼ��</param>
    /// <param name="targetPos">����Ч������Ʒ�����յ�</param>
    /// <param name="targetProp">�ȼ�</param>
    public void DistributeProp(Vector3 startPos,Vector3 target,int level)
    {
        GameObject w=Instantiate(DistributeRandomWeaponbyLevel(level), startPos, Quaternion.identity);
        w.transform.DOMove(target,0.8f);
    }
    /// <summary>
    /// ��ָ�����߷��͸���ұ������������Ķ���Ч��
    /// </summary>
    /// <param name="startPos">����Ч������Ʒ������ʼ��</param>
    /// <param name="targetPos">����Ч������Ʒ�����յ�</param>
    /// <param name="targetProp">Ҫ��õĵ���</param>
    public void DistributeProp(Vector3 startPos,Vector3 target,Prop_Data targetProp)
    {
        Prop_Data prop_Data = Instantiate(targetProp);
        StartCoroutine(prop_Data.OnDistributed(startPos,target));
        PropBackPackUIMgr.Instance.GetProp(prop_Data);
    }

    /// <summary>
    /// ��ָ����Ʒ���͸���ұ������������Ķ���Ч��
    /// </summary>
    /// <param name="startPos">����Ч������Ʒ������ʼ��</param>
    /// <param name="targetPos">����Ч������Ʒ�����յ�</param>
    /// <param name="targetProp">Ҫ��õĲ�Ʒ</param>
    public void DistributeColection(Vector3 startPos, Vector3 target, Collection_Data targetProp)
    {
        Collection_Data collection_Data = Instantiate(targetProp);
        StartCoroutine(collection_Data.OnDistributed(startPos, target));
        PropBackPackUIMgr.Instance.AddCollection(collection_Data);
    }

    /// <summary>
    /// ����ָ���ȼ��ĵ���
    /// </summary>
    /// <param name="level">�ȼ�</param>
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
    /// ��������
    /// </summary>
    public void DistributeDice(int Amount)
    { 
        PropBackPackUIMgr.Instance.GainDice(Amount);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void DistributeCoin(int Amount)
    { 
        PropBackPackUIMgr.Instance.GainDice(Amount);
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
                    Debug.Log("����1����Ʒ");
                }
                else
                {
                    objects = DistributeRandomPropbyLevel(1);
                    Debug.Log("����1������");
                }
                if(objects) StartCoroutine(objects.OnDistributed(target.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position));
            }

            else if (randomNumber < 80)
            {
                if (DateTime.Now.GetHashCode() % 2 == 0)
                {
                    objects = DistributeRandomCollectionbyLevel(2);
                    Debug.Log("����2����Ʒ");
                }
                else
                {
                    objects = DistributeRandomPropbyLevel(2);
                    Debug.Log("����2������");
                }
                if(objects)StartCoroutine(objects.OnDistributed(target.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position));
            }

            else
            {
                DistributeDice(1);
            }
        }
    }
}
