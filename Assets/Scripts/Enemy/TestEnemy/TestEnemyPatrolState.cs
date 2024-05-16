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

        testEnemy.transform.Translate(0, 1 * Time.deltaTime, 0);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
