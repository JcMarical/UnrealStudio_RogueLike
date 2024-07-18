using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Confuse", menuName = "Data/SpecialState/SS_Confuse", order = 4)]
public class SS_Confuse : SpecialState
{
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Confuse();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            ((Player)Target).inputDirection = -((Player)Target).inputDirection;
        }
        base.StateExit(StateList);
    }
}
