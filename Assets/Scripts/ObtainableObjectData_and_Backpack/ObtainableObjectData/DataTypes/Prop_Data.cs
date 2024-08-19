using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "Data/ObtainableObjects/Prop", order = 0)]
public class Prop_Data : ObtainableObjectData
{
    public void BeSoldOut()
    {
        base.BeSoldOut();
        PropBackPackUIMgr.Instance.ReMoveProp();
    }
}