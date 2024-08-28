using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;

[CreateAssetMenu(fileName = "PFunc_Kettle",menuName = "Data/ObtainableObjects/Func/Kettle", order = 0)]
public class PFunc_Kettle : PropFunc
{
    public float healthEffectValue = 1;
    public int DiceIncrease = 1;
    public override void OnAwake()
    {
        base.OnAwake();
        Player.Instance.RealMaxHealth += healthEffectValue;
        PropBackPackUIMgr.Instance.CurrenetDices += DiceIncrease;
    }

    public override void UseProp()
    {
        base.UseProp();
    }

    public override void Finish()
    {
        base.Finish();
    }
}
