using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赫尔墨斯羊
/// </summary>
public class Sheep : Enemy
{
    [Header("羊")]
    [Space(16)]
    [Tooltip("主人")] public Hermes master;
    [Tooltip("筹码")] public GameObject chip;

    protected override void Awake()
    {
        base.Awake();

        chaseState = new SheepChaseState(this, enemyFSM, this);
        attackState = new SheepAttackState(this, enemyFSM, this);
        deadState = new SheepDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = chaseState;

        base.OnEnable();
    }

    public void ChipAttack()
    {
        GameObject chip = Instantiate(this.chip, transform.position, Quaternion.identity);
        chip.GetComponent<AttackEnemy>().enemy = this;
        chip.GetComponent<SheepChip>().Initialized(3 * tileLength, (player.transform.position - transform.position).normalized);
    }
}
