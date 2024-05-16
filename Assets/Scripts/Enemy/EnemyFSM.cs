using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态机控制脚本
/// </summary>
public class EnemyFSM
{
    public EnemyState currentState;
    public EnemyState startState;

    /// <summary>
    /// 状态初始化方法，在OnEnable中调用
    /// </summary>
    /// <param name="state">初始状态</param>
    public void InitializeState(EnemyState state)
    {
        currentState = state;
        currentState.OnEnter();
    }

    /// <summary>
    /// 切换状态方法
    /// </summary>
    /// <param name="state">切换后的状态</param>
    public void ChangeState(EnemyState state)
    {
        currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }
}
