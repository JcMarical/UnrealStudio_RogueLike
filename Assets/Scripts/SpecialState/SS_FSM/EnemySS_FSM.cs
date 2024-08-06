using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySS_FSM : SS_FSM
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void AddState(string StateName,float Duration)
    {
        SpecialState newState = CreateNewState(SS_Mgr.Instance.GetType(StateName));
        newState.targetType = SpecialState.TargetType.Enemy;
        base.AddState(newState, Duration);
    }
}
