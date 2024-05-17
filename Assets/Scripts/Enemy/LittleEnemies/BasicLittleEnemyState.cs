using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                int i = Random.Range(0, 2); 
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
                enemy.PatrolMove(patrolDirection);
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
