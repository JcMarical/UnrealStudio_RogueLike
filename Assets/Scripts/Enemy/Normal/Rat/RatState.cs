using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (rat.IsPlayerInVisualRange() && !rat.isPatrolMove && waitTimer <= 0.1f)
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

    float waitTimer;
    float attackTimer;

    public RatChaseState(Enemy enemy, EnemyFSM enemyFSM, Rat rat) : base(enemy, enemyFSM)
    {
        this.rat = rat;
    }

    public override void OnEnter()
    {
        rat.isAttack = true;
        waitTimer = rat.attackCoolDown[0];
    }

    public override void LogicUpdate()
    {
        if (waitTimer > 0 && !rat.isAttack)
            waitTimer -= Time.deltaTime;
        
        if (waitTimer <= 0)
        {
            rat.isAttack = false;
            waitTimer = rat.attackCoolDown[0];
        }

        if (rat.isAttack && rat.isCollidePlayer)
        {
            rat.isAttack = false;
            waitTimer = rat.attackCoolDown[0];
        }
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
