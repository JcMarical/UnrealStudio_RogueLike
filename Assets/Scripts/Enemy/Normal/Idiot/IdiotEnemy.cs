using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdiotEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        patrolState = new IdiotStatePatrol(this, enemyFSM, this);
        chaseState = new IdiotStateChase(this, enemyFSM, this);
        attackState = new IdiotStateAttack(this, enemyFSM, this);
        deadState = new IdiotStateDead(this, enemyFSM, this);
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
        force = 300f;
    }

    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            enemyFSM.ChangeState(deadState);
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
