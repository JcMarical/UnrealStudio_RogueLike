using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleStatePYPPatrolState : BasicPatrolState
{
    CirclePYP testEnemy;

    public CircleStatePYPPatrolState(Enemy enemy, EnemyFSM enemyFSM, CirclePYP testEnemy) : base(enemy, enemyFSM)
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

        testEnemy.transform.Translate(0, 1 * Time.deltaTime, 0);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class CircleStatePYPChaseState : EnemyState
{
    CirclePYP testEnemy;

    public CircleStatePYPChaseState(Enemy enemy, EnemyFSM enemyFSM, CirclePYP testEnemy) : base(enemy, enemyFSM)
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

public class CircleStatePYPAttackState : EnemyState
{
    CirclePYP testEnemy;

    public CircleStatePYPAttackState(Enemy enemy, EnemyFSM enemyFSM, CirclePYP testEnemy) : base(enemy, enemyFSM)
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

public class CircleStatePYPDeadState : EnemyState
{
    CirclePYP testEnemy;

    public CircleStatePYPDeadState(Enemy enemy, EnemyFSM enemyFSM, CirclePYP testEnemy) : base(enemy, enemyFSM)
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

