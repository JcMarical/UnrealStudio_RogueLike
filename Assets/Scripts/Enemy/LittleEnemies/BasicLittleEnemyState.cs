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

/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
/// </summary>
public class BasicChaseState : EnemyState
{
    public BasicChaseState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void PhysicsUpdate()
    {

    }
}

/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
/// </summary>
public class BasicAttackState : EnemyState
{
    public BasicAttackState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
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
