using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 懒惰之龟的基础追击状态
/// </summary>
public class LazyTurtleStateChase : EnemyState
{
    public float timer;
    public LazyTurtleStateChase(Enemy enemy, EnemyFSM enemyFSM, LazyTurtleEnemy lazyTurtleEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        timer = 5f;
    }

    public override void LogicUpdate()
    {
        timer-=Time.deltaTime;
        if (timer < 0f)
        {
            enemyFSM.ChangeState(enemy.attackState);
        }
        enemy.AutoPath();
    }

    public override void PhysicsUpdate()
    {
        enemy.ChaseMove();
    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// 小怪的基础死亡状态
/// </summary>
public class LazyTurtleStateAttack : EnemyState
{
    public LazyTurtleStateAttack(Enemy enemy, EnemyFSM enemyFSM, LazyTurtleEnemy lazyTurtleEnemy) : base(enemy, enemyFSM)
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
/// 小怪的基础死亡状态
/// </summary>
public class LazyTurtleStateDead : EnemyState
{
    public LazyTurtleStateDead(Enemy enemy, EnemyFSM enemyFSM, LazyTurtleEnemy lazyTurtleEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isDead", true);
        enemy.gameObject.layer = 2;
        int dropsNum = Random.Range(0, enemy.drops.Length);
        if (enemy.drops.Length != 0)
        {
            enemy.InitializedDrops(Random.Range(enemy.dropsNumber[2 * dropsNum], enemy.dropsNumber[2 * dropsNum + 1]));
        }
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

