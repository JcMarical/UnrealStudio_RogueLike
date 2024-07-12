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

public class TestEnemyChaseState : EnemyState
{
    TestEnemy testEnemy;

    public TestEnemyChaseState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        testEnemy.ChaseMove();
    }

    public override void OnExit()
    {
        
    }
}

public class TestEnemyAttackState : EnemyState
{
    TestEnemy testEnemy;

    public TestEnemyAttackState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
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

public class TestEnemyDeadState : EnemyState
{
    TestEnemy testEnemy;

    public TestEnemyDeadState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
    {
        this.testEnemy = testEnemy;
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