using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保安
/// </summary>
public class Bouncer : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        patrolState = new BouncerPatrolState(this, enemyFSM, this);
        attackState = new BouncerAttackState(this, enemyFSM, this);
        deadState = new BouncerDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = patrolState;

        base.OnEnable();
    }

    protected override void Start()
    {
        force = 500f;
    }

}
