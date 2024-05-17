using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Enemy;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class BasicPatrolState : EnemyState
{
    private float timer;
    private Vector2 patrolDirection;
    private float[] patrolTime = { 0.6f, 1f, 1.4f };
    private bool isPatrol;

    public BasicPatrolState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {
        
    }

    public override void OnEnter()
    {
        timer = 1;
        isPatrol = false;
    }

    public override void LogicUpdate()
    {
        if (enemy.enemyType != EnemyType.Fort)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0 && isPatrol)
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
        }

        if (enemy.IsPlayerInVisualRange())
        {
            enemyFSM.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (enemy.enemyType != EnemyType.Fort)
        {
            if(isPatrol)
            {
                enemy.anim.SetBool("isMove", true);  //播放跑的动画并且随机向随机一个地方移动
                enemy.PatrolMove(patrolDirection);
            }
            else
            {
                enemy.anim.SetBool("isMove", false);  //停止播放跑的动画，播放Idle动画
            }
        }
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
    private float timer;
    private bool isRetreat;

    public BasicChaseState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        enemy.anim.SetBool("isMove", true);  //播放跑的动画

        timer = enemy.globalTimer;
        isRetreat = false;
    }

    public override void LogicUpdate()
    {
        if (enemy.enemyType == EnemyType.Ranged && enemy.IsPlayerInAttackRange())
        {
            isRetreat = true;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (enemy.enemyType == EnemyType.Fort)
            {
                if (enemy.IsPlayerInVisualRange())
                {
                    enemyFSM.ChangeState(enemy.attackState);
                }
            }
            else if(enemy.enemyType == EnemyType.Ranged)
            {
                if (enemy.IsPlayerInVisualRange() && !enemy.IsPlayerInAttackRange())
                {
                    enemyFSM.ChangeState(enemy.attackState);
                }
            }
            else
            {
                if (enemy.IsPlayerInAttackRange())
                {
                    enemyFSM.ChangeState(enemy.attackState);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if(enemy.enemyType != EnemyType.Fort)
        {
            if(enemy.enemyType == EnemyType.Ranged)
            {
                if (isRetreat)
                {
                    //TODO:敌人后撤逻辑
                }
            }
            else
            {
                //TODO:敌人追击逻辑
            }
        }
    }

    public override void OnExit()
    {
        enemy.anim.SetBool("isMove", false);
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
        enemy.anim.SetTrigger("attack");
        enemy.isAttack = true;
    }

    public override void LogicUpdate()
    {
        if (!enemy.isAttack)
        {
            enemyFSM.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        enemy.globalTimer = enemy.attackCoolDown[0];
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
        enemy.anim.SetBool("isDead", true);
        enemy.rb.velocity = Vector2.zero;
        enemy.gameObject.layer = 2;
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
