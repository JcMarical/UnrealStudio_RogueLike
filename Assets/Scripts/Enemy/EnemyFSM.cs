using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态机控制脚本
/// </summary>
public class EnemyFSM
{
    public EnemyState currentState; // 当前状态
    public EnemyState startState;   // 初始状态
    /// <summary>
    /// 状态初始化方法，在OnEnable中调用
    /// </summary>
    /// <param name="state">初始状态</param>
    public void InitializeState(EnemyState state)
    {
        currentState = state; // 将当前状态设置为初始状态
        currentState.OnEnter(); // 执行当前状态的OnEnter函数
    }

    /// <summary>
    /// 切换状态方法
    /// </summary>
    /// <param name="state">切换后的状态</param>
    public void ChangeState(EnemyState state)
    {
        currentState.OnExit(); // 执行当前状态的OnExit函数
        currentState = state; // 将当前状态设置为新状态
        currentState.OnEnter(); // 执行新状态的OnEnter函数
    }

}
