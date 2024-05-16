using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class BasicPatrolState : EnemyState
{
    public BasicPatrolState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
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
