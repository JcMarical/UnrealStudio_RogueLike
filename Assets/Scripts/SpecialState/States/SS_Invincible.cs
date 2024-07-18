using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;
[CreateAssetMenu(fileName = "SS_Invincible", menuName = "Data/SpecialState/SS_Invincible", order =12)]
public class SS_Invincible : SpecialState
{
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Invincible();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            ((Player)Target).isInvincible = false;
        }
        else
        {
            ((Enemy)Target).isInvincible = false;
        }
        base.StateExit(StateList);
    }
}
