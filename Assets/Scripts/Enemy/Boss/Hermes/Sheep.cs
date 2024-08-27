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
    [Space(16)]
    public List<GameObject> sheepList;

    protected override void Awake()
    {
        base.Awake();

        chaseState = new SheepChaseState(this, enemyFSM, this);
        attackState = new SheepAttackState(this, enemyFSM, this);
        deadState = new SheepDeadState(this, enemyFSM, this);

        sheepList = null;
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = chaseState;

        base.OnEnable();
    }

    protected override void Start()
    {
        force = 500f;
    }

    public void ChipAttack()
    {
        GameObject chip = Instantiate(this.chip, transform.position, Quaternion.identity);
        chip.GetComponent<AttackEnemy>().enemy = this;
        chip.GetComponent<SheepChip>().Initialize(2 * tileLength, (player.transform.position - transform.position).normalized);
    }
}
