using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class BouncerChaseState : EnemyState
{
    Bouncer bouncer;

    float timer;

    public BouncerChaseState(Enemy enemy, EnemyFSM enemyFSM, Bouncer bouncer) : base(enemy, enemyFSM)
    {
        this.bouncer = bouncer;
    }

    public override void OnEnter()
    {
        timer = bouncer.attackCoolDown[0];
        bouncer.acceleration = bouncer.chaseSpeed * 2;
    }

    public override void LogicUpdate()
    {
        bouncer.AutoPath();

        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0 && bouncer.IsPlayerInVisualRange())
            enemyFSM.ChangeState(bouncer.attackState);
        else
            enemyFSM.ChangeState(bouncer.patrolState);
    }

    public override void PhysicsUpdate()
    {
        bouncer.ChaseMove();
    }

    public override void OnExit()
    {

    }
}

public class BouncerAttackState : EnemyState
{
    Bouncer bouncer;

    float timer;
    bool isAttack;
    Vector2 direction;

    public BouncerAttackState(Enemy enemy, EnemyFSM enemyFSM, Bouncer bouncer) : base(enemy, enemyFSM)
    {
        this.bouncer = bouncer;
    }

    public override void OnEnter()
    {
        timer = 1;  //蓄力时间
        isAttack = false;
        bouncer.acceleration = 10;
    }

    public override void LogicUpdate()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0 && !isAttack)
        {
            direction = (bouncer.player.transform.position - bouncer.transform.position).normalized;
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
                    direction *= new Vector2(-1, 1); break;
                case 2:
                case 4:
                    direction *= new Vector2(1, -1); break;
                default:
                    break;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (isAttack)
            bouncer.Move(direction, bouncer.speed[0]/*冲撞速度*/);
    }

    public override void OnExit()
    {
        
    }
}

public class BouncerDeadState : EnemyState
{
    Bouncer bouncer;

    public BouncerDeadState(Enemy enemy, EnemyFSM enemyFSM, Bouncer bouncer) : base(enemy, enemyFSM)
    {
        this.bouncer = bouncer;
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
