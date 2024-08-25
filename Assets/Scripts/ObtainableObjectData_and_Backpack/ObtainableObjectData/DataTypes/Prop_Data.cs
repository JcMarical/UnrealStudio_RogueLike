using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "Data/ObtainableObjects/Prop", order = 0)]
public class Prop_Data : ObtainableObjectData
{
    public override void BeBought(Vector3 startPos)
    {
        base.BeBought(startPos);
        PropBackPackUIMgr.Instance.SetProps(this);
    }
    public void BeSoldOut()
    {
        base.BeSoldOut();
        PropBackPackUIMgr.Instance.ReMoveProp();
    }
}