using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;


/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class BasicPatrolState : EnemyState
{
    public Enemy enemy;
    private float timer;
    private Vector2 patrolDirection;
    private float patrolTime;
    public BasicPatrolState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
        patrolDirection = Random.insideUnitCircle; // 随机选择一个方向进行巡逻
        patrolDirection.Normalize(); // 将方向向量归一化

        // 随机巡逻时间
        patrolTime = Random.Range(0.6f, 1.4f);
    }

    public override void OnEnter()
    {
        enemy.anim.SetBool("TestChase", false);
        enemy.anim.SetBool("TestAttack", false);
        enemy.anim.SetBool("TestHurt", false);

        // 停留1秒
        timer = 1f;
    }

    public override void LogicUpdate()
    {
        switch (enemy.enemyType)
        {
            case EnemyType.Impact:
                if (timer>0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    // 原地不动结束，开始巡逻
                    if (patrolTime>0)
                    {
                        patrolTime-=Time.deltaTime;
                        enemy.Move(patrolDirection); // 使用敌人的移动方法向指定方向移动
                    }
                    else
                    {
                        patrolDirection = Random.insideUnitCircle; // 随机选择一个方向进行巡逻
                        patrolDirection.Normalize(); // 将方向向量归一化

                        // 随机巡逻时间
                        patrolTime = Random.Range(0.6f, 1.4f);
                        timer = 1;
                    }
 
                }
                break;
            case EnemyType.Melee:

            case EnemyType.Ranged:

            case EnemyType.Fort:

            case EnemyType.Boss:

            default:
                Debug.Log("未知的敌人类型");
                break;
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
