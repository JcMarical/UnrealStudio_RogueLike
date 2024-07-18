using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Hot", menuName = "Data/SpecialState/SS_Hot", order = 1)]
public class SS_Hot : SpecialState
{
    private float LastEffectTime = -10;
    public float EffectInterval_Player = 1;
    public float EffectInterval_Enemy = 1;
    public float PlayerHarm = 0.1f;
    public float EnemyHarm = 0.1f;
    public override void StateAwake()
    {
        base.StateAwake();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (targetType == TargetType.Player)
        {
            if (Time.time - LastEffectTime > EffectInterval_Player)
            { 
                LastEffectTime = Time.time;
                Target.SS_Hot(PlayerHarm);
            }
        }
        else
        {
            if (Time.time - LastEffectTime > EffectInterval_Enemy)
            {
                LastEffectTime = Time.time;
                Target.SS_Hot(EnemyHarm);
            }
        }
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        base.StateExit(StateList);
    }
}
