using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Mgr : TInstance<SS_Mgr>
{
    public GameObject Target;
    public SpecialState State;
    public float Duration;

    public void AddSpecialState(GameObject Target,SpecialState State,float Duration)
    {
        SS_FSM target;
        if (Target.GetComponent<Enemy>())
        {
            target = Target.GetComponent<EnemySS_FSM>();
            ((EnemySS_FSM)target).AddState(State,Duration);
        }
        else
        { 
            target = Target.GetComponent<PlayerSS_FSM>();
            ((PlayerSS_FSM)target).AddState(State,Duration);
        }
    }

    public void RemoveSpecialState(GameObject Target,SpecialState State)
    {
        SS_FSM target = Target.GetComponent<SS_FSM>();
        if (target)
        {
            target.RemoveState(State);
        }
    }

    public void RemoveAllSpecialState(GameObject Target)
    {
        SS_FSM target = Target.GetComponent<SS_FSM>();
        if (target)
        {
            target.RemoveAllState();
        }
    }
}
