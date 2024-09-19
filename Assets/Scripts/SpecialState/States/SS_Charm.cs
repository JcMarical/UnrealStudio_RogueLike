using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Charm", menuName = "Data/SpecialState/SS_Charm", order = 11)]
public class SS_Charm : SpecialState
{
    public float moveSpeed;
    public override void StateAwake()
    {
        base.StateAwake();
        if(targetType == TargetType.Enemy) return;
        moveSpeed = Player.Instance.RealPlayerSpeed;
    }

    public override void StateUpdate()
    {
        if (targetType == TargetType.Player)
        {
            base.StateUpdate();
            Target.SS_Charm(From.transform, moveSpeed);
        }
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            BindingChange.Instance.inputControl.Enable();
            ((Player)Target).playerAnimation.inputControl.Enable();
            ((Player)Target).gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        base.StateExit(StateList);
    }

    public override void CopyData(SpecialState StandardData)
    {
        base.CopyData(StandardData);
        moveSpeed = (StandardData as SS_Charm).moveSpeed;
    }
}
