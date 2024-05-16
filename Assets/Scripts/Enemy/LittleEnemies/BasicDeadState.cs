using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
/// </summary>
public class BasicDeadState : EnemyState
{
    public BasicDeadState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
