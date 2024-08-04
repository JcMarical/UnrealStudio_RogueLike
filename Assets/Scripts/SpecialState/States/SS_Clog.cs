using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;
[CreateAssetMenu(fileName = "SS_Clog", menuName = "Data/SpecialState/SS_Clog", order = 7)]
public class SS_Clog : SpecialState
{
    public float EffectValue;
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Clog(EffectValue);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            //((Player)Target).speedBonus = ((Player)Target).speedBonus * (1 / (1 - EffectValue));
            PlayerBuffMonitor.Instance.MoveSpeedBuff *= (1 / (1 - EffectValue));
        }
        else
        {
            ((Enemy)Target).speedMultiple = ((Enemy)Target).speedMultiple * (1 / (1 - EffectValue));
        }
        base.StateExit(StateList);
    }
}
