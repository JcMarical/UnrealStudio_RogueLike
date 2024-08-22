using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collection", menuName = "Data/ObtainableObjects/Collection", order = 0)]
public class Collection_Data : ObtainableObjectData
{
    public static Collection_Data NULLData
    {
        get
        {
            Collection_Data NCD = CreateInstance<Collection_Data>();
            NCD.Name = "无";
            NCD.ID = 0;
            NCD.Icon = null;
            NCD.Rarity = Rarities.N;
            NCD.PropDesc = "无";
            NCD.HowtoGet = "无";
            NCD.OtherDesc = "无";
            NCD.PropFunc = null;
            return NCD;
        }
    }

    public void BeSoldOut()
    { 
        base.BeSoldOut();
        PropBackPackUIMgr.Instance.ReMoveCollection(this);  
    }
}
