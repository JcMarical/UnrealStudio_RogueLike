using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Prunk", menuName = "Data/ObtainableObjects/Func/Prunk", order = 16)]
public class PFunc_Prunk : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        WeaponCtrl.Instance.OnDamage += Func;
    }

    public override void Finish()
    {
        base.Finish();
    }

    private void Func(GameObject target)
    {
        target.GetComponent<EnemySS_FSM>()?.AddState(SpecialState_Type.SS_Sticky,15);
    }
}
