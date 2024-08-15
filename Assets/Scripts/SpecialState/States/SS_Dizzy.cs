using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;
[CreateAssetMenu(fileName = "SS_Dizzy", menuName = "Data/SpecialState/SS_Dizzy", order = 8)]
public class SS_Dizzy : SpecialState
{
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Dizzy();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            BindingChange.Instance.inputControl.Enable();
        }
        else
        { 
            ((Enemy)Target).isDizzy = false;
        }
        base.StateExit(StateList);
    }
}
