using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赫尔墨斯牛
/// </summary>
public class Cow : Enemy
{
    [Header("牛")]
    [Space(16)]
    [Tooltip("主人")] public Hermes master;

    protected override void Awake()
    {
        base.Awake();

        chaseState = new CowChaseState(this, enemyFSM, this);
        deadState = new CowDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = chaseState;

        base.OnEnable();
    }
}
