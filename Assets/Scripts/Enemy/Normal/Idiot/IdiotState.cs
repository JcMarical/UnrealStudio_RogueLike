using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 小怪的基础巡逻状态
/// </summary>
public class IdiotStatePatrol : BasicPatrolState
{

    public IdiotStatePatrol(Enemy enemy, EnemyFSM enemyFSM, IdiotEnemy idiotEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


/// <summary>
/// 小怪的基础追击状态
/// </summary>
public class IdiotStateChase : EnemyState
{
    private float coolDownTimer;
    private float hatredTimer;
    private Vector2 chaseDirection;
    private Vector2 retreatDirection;

    public IdiotStateChase(Enemy enemy, EnemyFSM enemyFSM, IdiotEnemy idiotEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isMove", true);  //播放跑的动画

        coolDownTimer = enemy.globalTimer;
        hatredTimer = 2;
        chaseDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
    }

    public override void LogicUpdate()
    {
        //丢失仇恨切换到巡逻状态的逻辑判断
        if (!enemy.IsPlayerInVisualRange())
        {
            hatredTimer -= Time.deltaTime;
        }
        else
        {
            hatredTimer = 5;
        }

        if (hatredTimer <= 0)
        {
            enemyFSM.ChangeState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.ChaseMove();
    }

    public override void OnExit()
    {
        //enemy.anim.SetBool("isMove", false);
    }
}

/// <summary>
/// 小怪的攻击状态
/// </summary>
public class IdiotStateAttack : EnemyState
{
    public IdiotStateAttack(Enemy enemy, EnemyFSM enemyFSM, IdiotEnemy idiotEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {
        //丢失仇恨切换到巡逻状态的逻辑判断
        if (!enemy.IsPlayerInAttackRange())
        {
            enemyFSM.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// 小怪的死亡状态
/// </summary>
public class IdiotStateDead : EnemyState
{
    public IdiotStateDead(Enemy enemy, EnemyFSM enemyFSM, IdiotEnemy idiotEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isDead", true);
        enemy.gameObject.layer = 2;
        enemy.DestroyGameObject();
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
