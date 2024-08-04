using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Sticky", menuName = "Data/SpecialState/SS_Sticky", order = 5)]
public class SS_Sticky : SpecialState
{
    public float EffectValue;
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Sticky(EffectValue);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            //Player player = Target as Player;
            //player.speedBonus = player.speedBonus*(1/(1- EffectValue));
            PlayerBuffMonitor.Instance.MoveSpeedBuff *= (1 / (1 - EffectValue));
        }
        else
        { 
            Enemy enemy = Target as Enemy;
            enemy.speedMultiple = enemy.speedMultiple * (1 / (1 - EffectValue));
        }
        base.StateExit(StateList);
    }
}
