using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Fixation", menuName = "Data/SpecialState/SS_Fixation", order = 3)]
public class SS_Fixation : SpecialState
{
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Fixation();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }
    
    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            BindingChange.Instance.inputControl.GamePlay.Move.Enable();
            ((Player)Target).playerAnimation.inputControl.GamePlay.Move.Enable();
            BindingChange.Instance.inputControl.GamePlay.Dash.started += ((Player)Target).Dash;
        }
        else
        { 
            ((Enemy)Target).isFixation = false;
        }
        base.StateExit(StateList);
    }
}
