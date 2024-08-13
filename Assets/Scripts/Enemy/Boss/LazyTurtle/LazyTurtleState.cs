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
        enemy.currentSpeed = 0.8f;
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
/// 乌龟的口水子弹攻击状态
/// </summary>
public class LazyTurtleStateAttack : EnemyState
{
    LazyTurtleEnemy lazyTurtleEnemy;
    public LazyTurtleStateAttack(Enemy enemy, EnemyFSM enemyFSM, LazyTurtleEnemy lazyTurtleEnemy) : base(enemy, enemyFSM)
    {
        this.lazyTurtleEnemy = lazyTurtleEnemy;
    }

    public override void OnEnter()
    {
        enemy.currentSpeed = 0f;
    }

    public override void LogicUpdate()
    {
        lazyTurtleEnemy.TryAttack();
        enemyFSM.ChangeState(enemy.chaseState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// 乌龟的通杀状态
/// </summary>
public class LazyTurtleStateKill : EnemyState
{
    LazyTurtleEnemy lazyTurtleEnemy;
    public float timer;
    public LazyTurtleStateKill(Enemy enemy, EnemyFSM enemyFSM, LazyTurtleEnemy lazyTurtleEnemy) : base(enemy, enemyFSM)
    {
        this.lazyTurtleEnemy = lazyTurtleEnemy;
    }

    public override void OnEnter()
    {
        timer = 10f;
        enemy.currentSpeed = 0.8f;
        Vector3 playerPosition = enemy.player.transform.position;
        Vector2 Direction = (playerPosition - lazyTurtleEnemy.gameObject.transform.position).normalized;
        enemy.moveDirection = Direction;
    }

    public override void LogicUpdate()
    {
        timer-=Time.deltaTime;
        if (timer < 6f)
        {
            enemy.currentSpeed = 4f;
        }
        if (timer<2f)
        {
            enemy.currentSpeed = 0.8f;
        }
        if (timer < 0)
        {
            lazyTurtleEnemy.hit = 0;
            lazyTurtleEnemy.killThroughout = false;
            enemyFSM.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.Move();
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

