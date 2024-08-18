using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Cold", menuName = "Data/SpecialState/SS_Cold", order = 2)]
public class SS_Cold : SpecialState
{
    public float EffectValue_Player = 0.1f;
    public float EffectValue_Enemy = 0.1f;
    public override void StateAwake()
    {
        base.StateAwake();
        if (targetType == TargetType.Player) Target.SS_Freeze(EffectValue_Player);
        else Target.SS_Freeze(EffectValue_Enemy);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            //((Player)Target).intervalBonus = ((Player)Target).intervalBonus*(1/(1+ EffectValue_Player));
            PlayerBuffMonitor.Instance.AtkSpeedBuff *= (1/(1+ EffectValue_Player));
        }
        else
        { 
            ((Enemy)Target).coolDownMultiple = ((Enemy)Target).coolDownMultiple*(1/(1+ EffectValue_Enemy));
        }
        base.StateExit(StateList);
    }

    public override void CopyData(SpecialState FreshData)
    {
        base.CopyData(FreshData);
        EffectValue_Player = (FreshData as SS_Cold).EffectValue_Player;
        EffectValue_Enemy = (FreshData as SS_Cold).EffectValue_Enemy;
    }
}
