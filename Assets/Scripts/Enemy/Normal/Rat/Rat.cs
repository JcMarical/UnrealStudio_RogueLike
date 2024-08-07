using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 老鼠
/// </summary>
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
}
