using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectRoomMgr : TInstance<ObjectRoomMgr>
{
    [SerializeField]private Tilemap Object;

    [OdinSerialize] public List<ITradable> AllObtianableObjects = new();//储存所有可出售的物品
    [OdinSerialize] public List<List<ITradable>> ObtainableObjects_Leveled = new();//储存按稀有度分类的商品
    [OdinSerialize] private List<ITradable> Objects = new();
    [OdinSerialize] private List<Vector2> Pos = new();
    [OdinSerialize] private Dictionary<Vector2, ITradable> ObjectDic = new();
    [OdinSerialize] private Dictionary<Vector2, GameObject> _Object = new();
    [SerializeField]private GameObject ObjectContainer; 

    private void Awake()
    {
        SortTheObtainableObjectsByRarity();
        GetPos();
        ShowObjects();
    }

    private void SortTheObtainableObjectsByRarity()
    {
        for (int i = 0; i < (int)Rarities.UR; i++)
        {
            ObtainableObjects_Leveled.Add(new());
        }

        foreach (ITradable Obtain in AllObtianableObjects)
        {
            if (!PropBackPackUIMgr.Instance.CollectionDatas.Contains(Obtain as Collection_Data)) 
                ObtainableObjects_Leveled[(int)(Obtain as ObtainableObjectData).Rarity].Add(Obtain);
        }
    }

    private void GetPos()
    {
        int max_x = Object.cellBounds.xMax;
        int max_y = Object.cellBounds.yMax;
        for (int y = Object.cellBounds.yMin; y < max_y; y++)
        {
            for (int x = Object.cellBounds.xMin; x < max_x; x++)
            {
                if (Object.GetTile(new Vector3Int(x, y, 0)))
                {
                    Pos.Add(Object.CellToWorld(new Vector3Int(x,y,0)) + Object.cellSize / 2);
                }
            }
        }
    }

    ITradable GetRandomObjectwithRarityLimit(RarityandProbabilityOfObectRoomPerLayer RAP_Object)
    { 
        int RandomNumber = Random.Range(0, 100);
        var CurrentRAP = RAP_Object.Probability;
        Rarities Rarity = RAP_Object.minRarity;

        while (CurrentRAP[Rarity] < RandomNumber)
        {
            Rarity++;
        }

        while (ObtainableObjects_Leveled[(int)Rarity].Count == 0)
        {
            Rarity--;
        }

        return GetRandomObjectByRarityLimit(Rarity);
    }

    ITradable GetRandomObjectByRarityLimit(Rarities Rarity)
    {
        ITradable res = ObtainableObjects_Leveled[(int)Rarity][Random.Range(0, ObtainableObjects_Leveled[(int)Rarity].Count)];
        return Instantiate(res as ScriptableObject) as ITradable;
    }

    void ShowObjects()
    {
        foreach (var Pos in Pos)
        {
            var gam = GetRandomObjectwithRarityLimit(GameManager.Instance.GetCurrentRAP_ObjectRoom());
            ObjectDic.Add(Pos,gam);
            var obj = new GameObject((gam as ObtainableObjectData).Name);
            obj.transform.position = Pos;
            obj.transform.SetParent(ObjectContainer.transform);
            obj.AddComponent<SpriteRenderer>().sprite = (gam as ObtainableObjectData).Icon;
            obj.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("ObtainableObject");
            _Object.Add(Pos,obj);
        }
    }

    public void GetObject(GameObject Player,float DistanceLimit)
    {
        float CloestDistance = DistanceLimit;
        Vector2 CloestPos = new Vector2(float.MaxValue,float.MaxValue);
        foreach (var Pos in Pos)
        {
            if (Vector2.Distance(Pos, Player.transform.position) < CloestDistance)
            {
                CloestDistance = Vector2.Distance(Pos, Player.transform.position);
                CloestPos = Pos;
            }
        }

        if (CloestDistance == DistanceLimit)
        {
            return;
        }

        var obj = ObjectDic[CloestPos];
        if (obj as Collection_Data)
        {
            PropBackPackUIMgr.Instance.AddCollection(obj as Collection_Data);
            StartCoroutine((obj as Collection_Data).OnDistributed(CloestPos,Player.transform.position));
        }
        else
        {
            if (!PropBackPackUIMgr.Instance.GetProp(obj as Prop_Data))
            { 
                //TODO：道具栏已满，处理提示UI
                Debug.Log("道具满了_物品房警告");
            }
            StartCoroutine((obj as Prop_Data).OnDistributed(CloestPos, Player.transform.position));
        }

        Objects.Clear();
        ObjectDic.Clear();
        foreach (var gam in _Object)
        {
            Destroy(gam.Value);
        }
        _Object.Clear();
    }
}
