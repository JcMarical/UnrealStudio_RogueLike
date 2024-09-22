using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerSS_FSM : SS_FSM
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
    /// ͨ��״̬�����״̬��ָ��״̬��Դ
    /// </summary>
    /// <param name="StateName">״̬��</param>
    /// <param name="Duration">״̬����ʱ��</param>
    /// <param name="From">״̬��Դ</param>
    public void AddState(string StateName, float Duration,GameObject From)
    {
        SpecialState newState = CreateNewState(StateName);
        newState.targetType = SpecialState.TargetType.Player;
        base.AddState(newState, Duration,From);
    }

    /// <summary>
    /// ͨ��״̬ö�����״̬��ָ��״̬��Դ
    /// </summary>
    /// <param name="state_Type">״̬ö��</param>
    /// <param name="Duration">״̬����ʱ��(s)</param>
    /// <param name="From">״̬��Դ(s)</param>
    public void AddState(SpecialState_Type state_Type, float Duration, GameObject From)
    {
        SpecialState newState = CreateNewState(state_Type.ToString());
        newState.targetType = SpecialState.TargetType.Player;
        base.AddState(newState, Duration,From);
    }
}
