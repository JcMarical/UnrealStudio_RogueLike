using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蚊子的巡逻状态
/// </summary>
public class MosquitoPatrolState : BasicPatrolState
{
    Mosquito mosquito;

    public MosquitoPatrolState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        if (mosquito.IsPlayerInVisualRange())
            enemyFSM.ChangeState(mosquito.chaseState);
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}

/// <summary>
/// 蚊子的追击状态
/// </summary>
public class MosquitoChaseState : EnemyState
{
    Mosquito mosquito;

    Vector2 direction;

    public MosquitoChaseState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
    }

    public override void OnEnter()
    {
        mosquito.currentSpeed = mosquito.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        if (mosquito.isCollidePlayer)
            enemyFSM.ChangeState(mosquito.attackState);
    }

    public override void PhysicsUpdate()
    {
        mosquito.moveDirection = (mosquito.player.transform.position - mosquito.transform.position).normalized;
        mosquito.Move();
    }

    public override void OnExit()
    {
        
    }
}

/// <summary>
/// 蚊子的攻击状态（自爆）
/// </summary>
public class MosquitoAttackState : EnemyState
{
    Mosquito mosquito;

    public MosquitoAttackState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
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
/// 蚊子的死亡状态
/// </summary>
public class MosquitoDeadState : EnemyState
{
    Mosquito mosquito;

    public MosquitoDeadState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
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