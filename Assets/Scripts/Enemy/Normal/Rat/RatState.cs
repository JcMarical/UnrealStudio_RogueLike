using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RatPatrolState : BasicPatrolState
{
    Rat rat;

    public RatPatrolState(Enemy enemy, EnemyFSM enemyFSM, Rat rat) : base(enemy, enemyFSM)
    {
        this.rat = rat;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        if (rat.IsPlayerInVisualRange() && !rat.isPatrolMove)
            enemyFSM.ChangeState(rat.chaseState);

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

public class RatChaseState : EnemyState
{
    Rat rat;

    float basicMoveTime;
    float currentMoveTime;
    float waitTimer;
    float moveTimer;

    public RatChaseState(Enemy enemy, EnemyFSM enemyFSM, Rat rat) : base(enemy, enemyFSM)
    {
        this.rat = rat;
    }

    public override void OnEnter()
    {
        rat.isAttack = true;
        basicMoveTime = enemy.basicPatrolDistance / enemy.patrolSpeed;
        currentMoveTime = Random.Range(enemy.basicPatrolDistance - 1, enemy.basicPatrolDistance + 1) / enemy.patrolSpeed;
        moveTimer = currentMoveTime;
        waitTimer = rat.attackCoolDown[0];
    }

    public override void LogicUpdate()
    {
        if (!rat.isAttack && !rat.IsPlayerInVisualRange())
            enemyFSM.ChangeState(rat.patrolState);

        if (waitTimer > 0 && !rat.isAttack)
            waitTimer -= Time.deltaTime;

        if (moveTimer > 0 && rat.isAttack)
            moveTimer -= Time.deltaTime;
        
        if (waitTimer <= 0)
            rat.isAttack = true;

        if (rat.isAttack && rat.isCollidePlayer)
        {
            rat.isAttack = false;

            if (currentMoveTime > basicMoveTime * 2 || currentMoveTime < basicMoveTime * 0.5f)
                currentMoveTime = basicMoveTime;
            else
                currentMoveTime *= Random.Range(0.75f, 1.5f);
            moveTimer = currentMoveTime;

            waitTimer = rat.attackCoolDown[0];
        }

        if (moveTimer < 0)
        {
            rat.isAttack = false;

            if (currentMoveTime > basicMoveTime * 2 || currentMoveTime < basicMoveTime * 0.5f)
                currentMoveTime = basicMoveTime;
            else
                currentMoveTime *= Random.Range(0.75f, 1.5f);
            moveTimer = currentMoveTime;

            waitTimer = rat.attackCoolDown[0];
        }

        if (rat.isAttack)
            rat.AutoPath();
    }

    public override void PhysicsUpdate()
    {
        if (rat.isAttack)
            rat.ChaseMove();
    }

    public override void OnExit()
    {
        
    }
}

public class RatDeadState : EnemyState
{
    Rat rat;

    public RatDeadState(Enemy enemy, EnemyFSM enemyFSM, Rat rat) : base(enemy, enemyFSM)
    {
        this.rat = rat;
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
