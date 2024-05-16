using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPatrolState : EnemyState
{
    TestEnemy testEnemy;

    public TestEnemyPatrolState(Enemy enemy, EnemyFSM enemyFSM, TestEnemy testEnemy) : base(enemy, enemyFSM)
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
        testEnemy.transform.Translate(1 * Time.deltaTime, 0, 0);
    }

    public override void OnExit()
    {
        
    }
}
