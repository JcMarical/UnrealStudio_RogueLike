﻿using UnityEngine;
using static Enemy;
/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class PickpocketsStatePatrol : BasicPatrolState
{
    PickpocketsEnemy pickpocketsEnemy;
    protected Vector2 moveDirection;
    public float attackTime=0f;

    public PickpocketsStatePatrol(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {
        this.pickpocketsEnemy = pickpocketsEnemy;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.IsPlayerInVisualRange())
        {
            if (attackTime <= 0f && !pickpocketsEnemy.bullet)
            {
                attackTime = 1.5f;
                enemyFSM.ChangeState(enemy.attackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base .PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
/// </summary>
public class PickpocketsStateChase : EnemyState
{

    public PickpocketsStateChase(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
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
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
/// </summary>
public class PickpocketsStateAttack : EnemyState
{
    PickpocketsEnemy pickpocketsEnemy;
    public PickpocketsStateAttack(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {
        this.pickpocketsEnemy = pickpocketsEnemy;
    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        pickpocketsEnemy.TryAttack();
        enemyFSM.ChangeState(enemy.patrolState);
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
public class PickpocketsStateDead : EnemyState
{
    public PickpocketsStateDead(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isDead", true);
        enemy.gameObject.layer = 2;
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        enemy.rb.velocity = Vector2.zero;
    }

    public override void OnExit()
    {

    }
}
