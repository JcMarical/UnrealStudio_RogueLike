using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoPatrolState : BasicPatrolState
{
    Mosquito mosquito;

    public MosquitoPatrolState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        if (mosquito.IsPlayerInVisualRange())
            enemyFSM.ChangeState(mosquito.chaseState);
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}

public class MosquitoChaseState : EnemyState
{
    Mosquito mosquito;

    Vector2 direction;

    public MosquitoChaseState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        if (mosquito.IsPlayerInAttackRange())
            enemyFSM.ChangeState(mosquito.attackState);
    }

    public override void PhysicsUpdate()
    {
        direction = (mosquito.player.transform.position - mosquito.transform.position).normalized;
        mosquito.Move(direction, mosquito.chaseSpeed);
    }

    public override void OnExit()
    {
        
    }
}

public class MosquitoAttackState : EnemyState
{
    Mosquito mosquito;

    public MosquitoAttackState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
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

public class MosquitoDeadState : EnemyState
{
    Mosquito mosquito;

    public MosquitoDeadState(Enemy enemy, EnemyFSM enemyFSM, Mosquito mosquito) : base(enemy, enemyFSM)
    {
        this.mosquito = mosquito;
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