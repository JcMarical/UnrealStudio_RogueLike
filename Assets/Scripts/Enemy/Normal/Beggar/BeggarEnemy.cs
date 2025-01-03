﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeggarEnemy : Enemy
{
    public PropDistributor propDistributor;
    protected override void Awake()
    {
        base.Awake();

        patrolState = new BeggarStatePatrol(this, enemyFSM, this);
        chaseState = new BeggarStateChase(this, enemyFSM, this);
        attackState = new BeggarStateAttack(this, enemyFSM, this);
        deadState = new BeggarStateDead(this, enemyFSM, this);
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
        if (currentHealth <= 0)
        {
            
            propDistributor.DistributeCoin(Random.Range(coinNumber.min, coinNumber.max+1));
            enemyFSM.ChangeState(deadState);
            return;
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
