using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Burn", menuName = "Data/SpecialState/SS_Burn", order = 6)]
public class SS_Burn : SpecialState
{
    public override void StateAwake()
    {
        base.StateAwake();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (targetType == TargetType.Player) { Target.SS_Burn(); } //ִ���������״̬
        else;//ִ�е�������״̬
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        base.StateExit(StateList);
    }
}
