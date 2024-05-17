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

public class TestEnemyAttackState : BasicChaseState
{
    TestEnemy testEnemy;

    public TestEnemyAttackState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
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

public class TestEnemyDeadState : BasicChaseState
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