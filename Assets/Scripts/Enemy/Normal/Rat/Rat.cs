using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        patrolState = new RatPatrolState(this, enemyFSM, this);
        chaseState = new RatChaseState(this, enemyFSM, this);
        deadState = new RatDeadState(this, enemyFSM, this);
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }
}
