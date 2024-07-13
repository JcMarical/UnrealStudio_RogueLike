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
public class BeggarStatePatrol : EnemyState
{
    private float timer;
    private Vector2 patrolDirection;
    private float[] patrolTime = { 0.6f, 1f, 1.4f };
    private bool isPatrol;

    public BeggarStatePatrol(Enemy enemy, EnemyFSM enemyFSM,BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        timer = 1;
        isPatrol = false;
    }

    public override void LogicUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0 && isPatrol)
        {
            timer = 1;
            isPatrol = false;
        }
        else
        {
            int i = Random.Range(0, 3);
            timer = patrolTime[i];  //随机巡逻时间
            float patrolDistance = enemy.patrolSpeed * patrolTime[i];   //计算巡逻距离

            do
            {
                patrolDirection = Random.insideUnitCircle; // 随机选择一个方向进行巡逻
                patrolDirection.Normalize(); // 将方向向量归一化
            } while (Physics2D.Raycast(enemy.transform.position, patrolDirection, patrolDistance, enemy.obstacleLayer));    //当巡逻路线上有障碍物时重新随机巡逻方向

            isPatrol = true;
        }

        if (enemy.IsPlayerInVisualRange())
        {
            enemyFSM.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {

        if (isPatrol)
        {
            //enemy.anim.SetBool("isMove", true);  //播放跑的动画并且随机向随机一个地方移动
            enemy.Move(patrolDirection,enemy.patrolSpeed);
        }
        else
        {
            //enemy.anim.SetBool("isMove", false);  //停止播放跑的动画，播放Idle动画
        }
    }

    public override void OnExit()
    {

    }

}

/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
/// </summary>
public class BeggarStateChase : EnemyState
{
    private float coolDownTimer;
    private float hatredTimer;
    private bool isRetreat;
    private Vector2 chaseDirection;
    private Vector2 retreatDirection;

    public BeggarStateChase(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isMove", true);  //播放跑的动画

        coolDownTimer = enemy.globalTimer;
        hatredTimer = 2;
        chaseDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
        isRetreat = false;
    }

    public override void LogicUpdate()
    {
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
public class BeggarStateAttack : EnemyState
{
    public BeggarStateAttack(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
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
public class BeggarStateDead : EnemyState
{
    public BeggarStateDead(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
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
