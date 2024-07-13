using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class IdiotStatePatrol : EnemyState
{
    private float timer;
    private Vector2 patrolDirection;
    private float[] patrolTime = { 0.6f, 1f, 1.4f };
    private bool isPatrol;

    public IdiotStatePatrol(Enemy enemy, EnemyFSM enemyFSM, IdiotEnemy idiotEnemy) : base(enemy, enemyFSM)
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
public class IdiotStateChase : EnemyState
{
    private float coolDownTimer;
    private float hatredTimer;
    private float retreatTimer;
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
        retreatTimer = 0.5f;
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
        enemy.ChaseMove(enemy.chaseSpeed);
    }

    public override void OnExit()
    {
        //enemy.anim.SetBool("isMove", false);
    }
}

/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
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
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
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
