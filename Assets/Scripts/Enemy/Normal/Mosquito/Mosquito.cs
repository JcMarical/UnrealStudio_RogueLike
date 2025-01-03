using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蚊子
/// </summary>
public class Mosquito : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        patrolState = new MosquitoPatrolState(this, enemyFSM, this);
        chaseState = new MosquitoChaseState(this, enemyFSM, this);
        attackState = new MosquitoAttackState(this, enemyFSM, this);
        deadState = new MosquitoDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = patrolState;

        base.OnEnable();
    }

    protected override void Start()
    {
        force = 0f;
    }
}
