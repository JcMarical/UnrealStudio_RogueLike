using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����״̬�����ƽű�
/// </summary>
public class EnemyFSM
{
    public EnemyState currentState;
    public EnemyState startState;

    /// <summary>
    /// ״̬��ʼ����������OnEnable�е���
    /// </summary>
    /// <param name="state">��ʼ״̬</param>
    public void InitializeState(EnemyState state)
    {
        currentState = state;
        currentState.OnEnter();
    }

    /// <summary>
    /// �л�״̬����
    /// </summary>
    /// <param name="state">�л����״̬</param>
    public void ChangeState(EnemyState state)
    {
        currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }
}
