using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySS_FSM : SS_FSM
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// ͨ��״̬�����״̬
    /// </summary>
    /// <param name="StateName">״̬��</param>
    /// <param name="Duration">״̬����ʱ��(s)</param>
    public void AddState(string StateName,float Duration)
    {
        SpecialState newState = CreateNewState(StateName);
        newState.targetType = SpecialState.TargetType.Enemy;
        base.AddState(newState, Duration);
    }

    /// <summary>
    /// ͨ��״̬ö�����״̬
    /// </summary>
    /// <param name="state_Type">״̬ö��</param>
    /// <param name="Duration">״̬����ʱ��(s)</param>
    public void AddState(SpecialState_Type state_Type,float Duration)
    {
        SpecialState newState = CreateNewState(state_Type.ToString());
        newState.targetType = SpecialState.TargetType.Enemy;
        base.AddState(newState, Duration);
    }
}
