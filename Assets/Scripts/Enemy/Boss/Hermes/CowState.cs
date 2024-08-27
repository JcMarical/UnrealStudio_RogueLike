using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赫尔墨斯牛的追击状态
/// </summary>
public class CowChaseState : EnemyState
{
    Cow cow;

    public CowChaseState(Enemy enemy, EnemyFSM enemyFSM, Cow cow) : base(enemy, enemyFSM)
    {
        this.cow = cow;
    }

    public override void OnEnter()
    {
        cow.currentSpeed = cow.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        cow.AutoPath();
    }

    public override void PhysicsUpdate()
    {
        cow.ChaseMove();
    }

    public override void OnExit()
    {
        cow.currentSpeed = 0;
        cow.moveDirection = Vector2.zero;
    }
}

/// <summary>
/// 赫尔墨斯牛的死亡状态
/// </summary>
public class CowDeadState : BasicDeadState
{
    Cow cow;

    public CowDeadState(Enemy enemy, EnemyFSM enemyFSM, Cow cow) : base(enemy, enemyFSM)
    {
        this.cow = cow;
    }

    public override void OnEnter()
    {
        if (cow.master != null)
        {
            cow.master.RemoveCow(cow);
            cow.master.currentHealth -= cow.master.currentHealth / 20;
            if (cow.master.currentHealth < cow.master.currentHealth / 2)
                cow.master.enemyFSM.ChangeState(cow.master.lyreShieldState);
        }

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
