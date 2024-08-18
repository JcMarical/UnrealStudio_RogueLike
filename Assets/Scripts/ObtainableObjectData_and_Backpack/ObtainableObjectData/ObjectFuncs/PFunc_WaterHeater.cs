using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;

[CreateAssetMenu(fileName = "PFunc_WaterHeater", menuName = "Data/ObtainableObjects/Func/WaterHeater", order = 1)]
public class PFunc_WaterHeater : PropFunc
{
    public float healthEffectValue = 2;
    public int DiceIncrease = 2;
    public override void OnAwake()
    {
        base.OnAwake();
        Player.Instance.realMaxHealth += healthEffectValue;
        PropBackPackUIMgr.Instance.Dices.Amount += DiceIncrease;
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
