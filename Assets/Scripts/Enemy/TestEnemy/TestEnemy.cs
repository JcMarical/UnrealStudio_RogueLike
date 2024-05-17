using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        patrolState = new TestEnemyPatrolState(this, enemyFSM, this);
        chaseState = new TestEnemyChaseState(this, enemyFSM, this);
        attackState = new TestEnemyAttackState(this, enemyFSM, this);
        deadState = new TestEnemyDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = patrolState;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
