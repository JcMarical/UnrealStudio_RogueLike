using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Acide", menuName = "Data/SpecialState/SS_Acide", order = 1)]
public class SS_Acide : SpecialState
{
    [SerializeField]private float LastEffectTime = -10;
    public float EffectInterval_Player = 2;
    public float EffectInterval_Enemy = 1;
    public float PlayerHarm = 0.1f;
    public float EnemyHarm = 0.1f;
    public override void StateAwake()
    {
        base.StateAwake();
        Debug.Log(Time.time);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (targetType == TargetType.Player)
        {
            if (Time.time - LastEffectTime > EffectInterval_Player)
            {
                Debug.Log("Acide + ʱ�䣺" + Time.timeAsDouble);
                LastEffectTime = Time.time;
                Target.SS_Acide(PlayerHarm);
            }
        }
        else
        {
            if (Time.time - LastEffectTime > EffectInterval_Enemy)
            {
                LastEffectTime = Time.time;
                Target.SS_Acide(EnemyHarm);
            }
        }
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        base.StateExit(StateList);
    }

    public override void CopyData(SpecialState FreshData)
    {
        base.CopyData(FreshData);
        PlayerHarm = (FreshData as SS_Acide).PlayerHarm;
        EnemyHarm = (FreshData as SS_Acide).EnemyHarm;
        EffectInterval_Player = (FreshData as SS_Acide).EffectInterval_Player;
        EffectInterval_Enemy = (FreshData as SS_Acide).EffectInterval_Enemy;
    }
}
