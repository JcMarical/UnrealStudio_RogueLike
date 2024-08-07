﻿using UnityEngine;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class SlotMachinesStatePatrol : EnemyState
{
    float time = 0f;
    bool Crazy=false;
    public SlotMachinesStatePatrol(Enemy enemy, EnemyFSM enemyFSM, SlotMachinesEnemy slotMachinesEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {
        if (time <= 0f)
        {
            if (enemy.IsPlayerInVisualRange())
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
        }
        else
        {
            time-=Time.deltaTime;
        }
    }

    public override void PhysicsUpdate()
    {

    }
    public void SetCrazy()
    {
        Crazy = true;
    }
    public override void OnExit()
    {
        time = 2f;
        if(Crazy)
        {
            time = 1f;
        }
    }
}

/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
/// </summary>
public class SlotMachinesStateChase : EnemyState
{
    public SlotMachinesStateChase(Enemy enemy, EnemyFSM enemyFSM, SlotMachinesEnemy slotMachinesEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}


/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
/// </summary>
public class SlotMachinesStateAttack : EnemyState
{
    SlotMachinesStatePatrol patrolState; // 引用巡逻状态的实例
    SlotMachinesEnemy slotMachinesEnemy;
    float timeBetweenBullets = 0.2f; // 子弹之间的时间间隔
    float timeUntilNextAttack = 0f;
    int num; // 每次大发射时确定的子弹数量
    int attacksCount = 0;
    int T=0;
    public SlotMachinesStateAttack(Enemy enemy, EnemyFSM enemyFSM, SlotMachinesEnemy slotMachinesEnemy, SlotMachinesStatePatrol patrolState) : base(enemy, enemyFSM)
    {
        this.slotMachinesEnemy = slotMachinesEnemy;
        this.patrolState = patrolState;
    }

    public override void OnEnter()
    {
        num = Random.Range(T, 4);
        if (num == 0)
        {
            T = 1;
            patrolState.SetCrazy(); // 修改 Crazy 的值为 true
            enemyFSM.ChangeState(enemy.patrolState);
        }
        timeUntilNextAttack = -1f;
        attacksCount = 0; // 初始化攻击次数计数器
    }

    public override void LogicUpdate()
    {
        if (timeUntilNextAttack <= 0f)
        {
            if (attacksCount < num)
            {
                slotMachinesEnemy.TryAttack();
                attacksCount++;
                timeUntilNextAttack = timeBetweenBullets; // 设置子弹之间的间隔
            }
        }
        else
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
        if (attacksCount>=num)
        {
            enemyFSM.ChangeState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
    }
}

/// <summary>
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
/// </summary>
public class SlotMachinesStateDead : EnemyState
{
    public SlotMachinesStateDead(Enemy enemy, EnemyFSM enemyFSM, SlotMachinesEnemy SlotMachinesEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
