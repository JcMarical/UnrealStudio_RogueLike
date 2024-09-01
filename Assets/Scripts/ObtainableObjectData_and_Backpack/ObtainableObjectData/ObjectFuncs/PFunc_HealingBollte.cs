using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;

[CreateAssetMenu(fileName = "PFunc_HealingBollte", menuName = "Data/ObtainableObjects/Func/HealingBollte", order = 3)]
public class PFunc_HealingBollte : PropFunc
{
    public float healingValue = 1;
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.realPlayerHealth += healingValue;
    }

    public override void Finish()
    {
        base.Finish();
    }
}
