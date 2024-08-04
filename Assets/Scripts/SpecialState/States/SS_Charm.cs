using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Charm", menuName = "Data/SpecialState/SS_Charm", order = 11)]
public class SS_Charm : SpecialState
{
    public Transform target;
    public float moveSpeed;
    public override void StateAwake()
    {
        base.StateAwake();
        if(targetType == TargetType.Enemy) return;
        moveSpeed = Player.Instance.realPlayerSpeed;
    }

    public override void StateUpdate()
    {
        if (targetType == TargetType.Player)
        {
            base.StateUpdate();
            Target.SS_Charm(target, moveSpeed);
        }
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            ((Player)Target).inputControl.Enable();
            ((Player)Target).playerAnimation.inputControl.Enable();
        }
        base.StateExit(StateList);
    }
}
