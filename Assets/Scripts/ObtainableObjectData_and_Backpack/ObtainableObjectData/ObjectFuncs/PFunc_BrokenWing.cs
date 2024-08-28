using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_BrokenWing", menuName = "Data/ObtainableObjects/Func/BrokenWing", order = 15)]
public class PFunc_BrokenWing : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
        UseProp();
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.RealPlayerSpeed += 0.1f;
        PropBackPackUIMgr.Instance.WhenDiceBeUesed += Func; 
    }

    public override void Finish()
    {
        base.Finish();
    }

    void Func()
    {
        Player.Instance.RealPlayerSpeed += 0.05f;
    }
}
