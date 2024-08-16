using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;
[CreateAssetMenu(fileName = "SS_Hurry", menuName = "Data/SpecialState/SS_Hurry", order = 9)]
public class SS_Hurry : SpecialState
{
    public float EffectValue;
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Hurry(EffectValue);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            //((Player)Target).speedBonus = ((Player)Target).speedBonus * (1 / (1 + EffectValue));
            PlayerBuffMonitor.Instance.MoveSpeedBuff *= (1/(1+EffectValue));            
        }
        else
        {
            ((Enemy)Target).speedMultiple = ((Enemy)Target).speedMultiple * (1 / (1 + EffectValue));
        }
        base.StateExit(StateList);
    }

    public override void CopyData(SpecialState FreshData)
    {
        base.CopyData(FreshData);
        EffectValue = (FreshData as SS_Hurry).EffectValue;
    }
}
