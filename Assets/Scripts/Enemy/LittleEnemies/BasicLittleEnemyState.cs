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
                timer -= Time.deltaTime;
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
    }

    public override void PhysicsUpdate()
    {
        if (enemy.enemyType != EnemyType.Fort)
        {
            if(isPatrol)
            {
                enemy.anim.SetBool("TestRun", true);  //播放跑的动画并且随机向随机一个地方移动
                enemy.PatrolMove(patrolDirection);
            }
            else
            {
                enemy.anim.SetBool("TestIdle", false);  //停止播放跑的动画，播放Idle动画
            }
        }
        if (enemy.IsPlayerInVisualRange())
        {
            //切换追击状态
        }
    }

    public override void OnExit()
    {
        enemy.anim.SetBool("TestRun", false);
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
        enemy.anim.SetBool("TestRun", true);  //播放跑的动画
    }

    public override void OnExit()
    {
        enemy.anim.SetBool("TestRun", false);
    }

    public override void PhysicsUpdate()
    {
        //向玩家移动，避开障碍物

        if (enemy.enemyType != EnemyType.Fort)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                //切换攻击状态
            }
        }
    }
}

/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
/// </summary>
public class BasicAttackState : EnemyState
{
    private float timer;
    public float AttackTime;    //动画事件中停止攻击动作，AttackTime可以作为攻击后摇
    public BasicAttackState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        enemy.anim.SetBool("TestAttack", true);
        timer = AttackTime;   
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        if (timer>0f)
        {
            timer-=Time.deltaTime;
        }
        else
        {
            if (enemy.enemyType != EnemyType.Fort)  //还在攻击范围就继续攻击
            {
                if (enemy.IsPlayerInAttackRange())
                {
                    enemy.anim.SetBool("TestAttack", true);
                    timer = AttackTime;  
                }
                else
                {
                    //切换巡逻状态
                }
            }
        }
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
    private float timer;
    public BasicDeadState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        enemy.anim.SetBool("TestAttack", false);  //攻击的时候被打死停止攻击动画
        enemy.anim.SetBool("TestDeath", true);
        timer = 1f;
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        if (timer>0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            enemy.DestroyGameObject();
        }
    }

    public override void OnExit()
    {

    }
}
