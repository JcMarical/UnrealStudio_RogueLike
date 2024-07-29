using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;
[CreateAssetMenu(fileName = "SS_Sticky", menuName = "Data/SpecialState/SS_Injury", order = 13)]
public class SS_Injury : SpecialState
{
    public float EffectValue;
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Injury(EffectValue);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            //TODO : 撤销效果
        }
        else
        {
            //TODO : 撤销效果
        }
        base.StateExit(StateList);
    }
}
