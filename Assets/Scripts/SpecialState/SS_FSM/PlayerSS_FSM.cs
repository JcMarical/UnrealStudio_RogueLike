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
    /// 通过状态名添加状态并指定状态来源
    /// </summary>
    /// <param name="StateName">状态名</param>
    /// <param name="Duration">状态持续时间</param>
    /// <param name="From">状态来源</param>
    public void AddState(string StateName, float Duration,GameObject From)
    {
        SpecialState newState = CreateNewState(StateName);
        newState.targetType = SpecialState.TargetType.Player;
        base.AddState(newState, Duration,From);
    }

    /// <summary>
    /// 通过状态枚举添加状态并指定状态来源
    /// </summary>
    /// <param name="state_Type">状态枚举</param>
    /// <param name="Duration">状态持续时间(s)</param>
    /// <param name="From">状态来源(s)</param>
    public void AddState(SpecialState_Type state_Type, float Duration, GameObject From)
    {
        SpecialState newState = CreateNewState(state_Type.ToString());
        newState.targetType = SpecialState.TargetType.Player;
        base.AddState(newState, Duration,From);
    }
}
