using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保安的巡逻状态
/// </summary>
public class BouncerPatrolState : BasicPatrolState
{
    Bouncer bouncer;

    public BouncerPatrolState(Enemy enemy, EnemyFSM enemyFSM, Bouncer bouncer) : base(enemy, enemyFSM)
    {
        this.bouncer = bouncer;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        if (bouncer.IsPlayerInVisualRange())
            enemyFSM.ChangeState(bouncer.attackState);

        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base .PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

/// <summary>
/// 保安的攻击状态（冲撞）
/// </summary>
public class BouncerAttackState : EnemyState
{
    Bouncer bouncer;

    float timer;
    bool isAttack;

    public BouncerAttackState(Enemy enemy, EnemyFSM enemyFSM, Bouncer bouncer) : base(enemy, enemyFSM)
    {
        this.bouncer = bouncer;
    }

    public override void OnEnter()
    {
        timer = 1;  //蓄力时间
        bouncer.currentSpeed = 0;
        bouncer.moveDirection = Vector2.zero;
        isAttack = false;
        bouncer.acceleration = 10;
    }

    public override void LogicUpdate()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0 && !isAttack)
        {
            bouncer.moveDirection = (bouncer.player.transform.position - bouncer.transform.position).normalized;
            bouncer.currentSpeed = bouncer.otherSpeed[0];    //冲撞速度
            isAttack = true;
            timer = 3;  //冲撞时间
        }
        else
            enemyFSM.ChangeState(bouncer.patrolState);

        if (bouncer.isCollideWall)
        {
            switch (bouncer.collideDirection)
            {
                case 1: 
                case 3:
                    bouncer.moveDirection *= new Vector2(-1, 1); break;
                case 2:
                case 4:
                    bouncer.moveDirection *= new Vector2(1, -1); break;
                default:
                    break;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (isAttack)
            bouncer.Move();
    }

    public override void OnExit()
    {
        bouncer.currentSpeed = 0;
        bouncer.moveDirection = Vector2.zero;
    }
}

/// <summary>
/// 保安的死亡状态
/// </summary>
public class BouncerDeadState : BasicDeadState
{
    Bouncer bouncer;

    public BouncerDeadState(Enemy enemy, EnemyFSM enemyFSM, Bouncer bouncer) : base(enemy, enemyFSM)
    {
        this.bouncer = bouncer;
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
