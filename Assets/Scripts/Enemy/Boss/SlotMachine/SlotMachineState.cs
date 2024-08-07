using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 老虎机的苹果状态
/// </summary>
public class SlotMachineAppleState : EnemyState
{
    SlotMachine slotMachine;

    float timer;
    bool isDraw;

    public SlotMachineAppleState(Enemy enemy, EnemyFSM enemyFSM, SlotMachine slotMachine) : base(enemy, enemyFSM)
    {
        this.slotMachine = slotMachine;
    }

    public override void OnEnter()
    {
        slotMachine.currentSpeed = slotMachine.patrolSpeed;
        timer = 20;
        isDraw = false;
    }

    public override void LogicUpdate()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0 && !isDraw)
        {
            //TODO: 播放抽奖动画
            isDraw = true;
            timer = 1.5f;
        }
        else
            enemyFSM.ChangeState(slotMachine.DrawLottery());

        slotMachine.AutoPath();
    }

    public override void PhysicsUpdate()
    {
        slotMachine.ChaseMove();
    }

    public override void OnExit()
    {
        slotMachine.moveDirection = Vector2.zero;
        slotMachine.currentSpeed = 0;
    }
}

/// <summary>
/// 老虎机的梨子状态
/// </summary>
public class SlotMachinePearState : EnemyState
{
    SlotMachine slotMachine;

    float timer;
    bool isDraw;

    public SlotMachinePearState(Enemy enemy, EnemyFSM enemyFSM, SlotMachine slotMachine) : base(enemy, enemyFSM)
    {
        this.slotMachine = slotMachine;
    }

    public override void OnEnter()
    {
        slotMachine.currentSpeed = slotMachine.chaseSpeed;
        timer = 20;
        isDraw = false;
    }

    public override void LogicUpdate()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0 && !isDraw)
        {
            //TODO: 播放抽奖动画
            isDraw = true;
            timer = 1.5f;
        }
        else
            enemyFSM.ChangeState(slotMachine.DrawLottery());

        slotMachine.AutoPath();
    }

    public override void PhysicsUpdate()
    {
        slotMachine.ChaseMove();
    }

    public override void OnExit()
    {
        slotMachine.moveDirection = Vector2.zero;
        slotMachine.currentSpeed = 0;
    }
}

/// <summary>
/// 老虎机的葡萄状态
/// </summary>
public class SlotMachineGrapeState : BasicPatrolState
{
    SlotMachine slotMachine;

    float drawTimer;
    float attackTimer;
    bool isDraw;

    public SlotMachineGrapeState(Enemy enemy, EnemyFSM enemyFSM, SlotMachine slotMachine) : base(enemy, enemyFSM)
    {
        this.slotMachine = slotMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        drawTimer = 20;
        attackTimer = slotMachine.attackCoolDown[0];
        isDraw = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //抽奖
        if (drawTimer > 0)
            drawTimer -= Time.deltaTime;
        else if (drawTimer <= 0 && !isDraw)
        {
            //TODO: 播放抽奖动画
            isDraw = true;
            drawTimer = 1.5f;
        }
        else
            enemyFSM.ChangeState(slotMachine.DrawLottery());

        //攻击
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else
            slotMachine.GrapeAttack();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

/// <summary>
/// 老虎机的西瓜状态
/// </summary>
public class SlotMachineWatermelonState : EnemyState
{
    SlotMachine slotMachine;

    float timer;
    bool isDraw;

    public SlotMachineWatermelonState(Enemy enemy, EnemyFSM enemyFSM, SlotMachine slotMachine) : base(enemy, enemyFSM)
    {
        this.slotMachine = slotMachine;
    }

    public override void OnEnter()
    {
        slotMachine.currentSpeed = 0;
        slotMachine.moveDirection = Vector2.zero;
        timer = 3.5f;
        isDraw = false;
    }

    public override void LogicUpdate()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0 && !isDraw)
        {
            //TODO: 播放抽奖动画
            isDraw = true;
            timer = 1.5f;
        }
        else
            enemyFSM.ChangeState(slotMachine.DrawLottery());
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        slotMachine.currentSpeed = 0;
        slotMachine.moveDirection = Vector2.zero;
    }
}

/// <summary>
/// 老虎机的死亡状态
/// </summary>
public class SlotMachineDeadState : BasicDeadState
{
    SlotMachine slotMachine;

    public SlotMachineDeadState(Enemy enemy, EnemyFSM enemyFSM, SlotMachine slotMachine) : base(enemy, enemyFSM)
    {
        this.slotMachine = slotMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
