using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPatrolState : BasicPatrolState
{
    TestEnemy testEnemy;

    public TestEnemyPatrolState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
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

public class TestEnemyChaseState : BasicChaseState
{
    TestEnemy testEnemy;

    public TestEnemyChaseState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
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

public class TestEnemyAttackState : BasicAttackState
{
    TestEnemy testEnemy;
    private float timer;
    private bool isDash;
    private bool hasDashed;
    private Vector2 dashDirection;

    public TestEnemyAttackState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        enemy.rb.velocity = Vector2.zero;
        timer = 1;
        isDash = false;
        hasDashed = false;
        dashDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && !isDash && !hasDashed)
        {
            isDash = true;
            timer = 0.5f;
        }

        if (timer <= 0 && isDash && !hasDashed)
        {
            isDash = false;
            hasDashed = true;
            timer = 0.5f;
        }

        if (timer <= 0 && hasDashed)
        {
            enemyFSM.ChangeState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isDash)
        {
            enemy.transform.Translate(dashDirection * 10 * Time.deltaTime);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class TestEnemyDeadState : BasicDeadState
{
    TestEnemy testEnemy;

    public TestEnemyDeadState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
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