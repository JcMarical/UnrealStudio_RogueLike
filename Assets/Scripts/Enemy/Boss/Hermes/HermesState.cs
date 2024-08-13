using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesSummonState : EnemyState
{
    Hermes hermes;

    public HermesSummonState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
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

public class HermesCaduceusState : EnemyState
{
    Hermes hermes;

    public HermesCaduceusState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
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

public class HermesLyreShieldState : EnemyState
{
    Hermes hermes;

    public HermesLyreShieldState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
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

public class HermesLyreBarrageState : EnemyState
{
    Hermes hermes;

    public HermesLyreBarrageState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
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

public class HermesDeadState : BasicDeadState
{
    Hermes hermes;

    public HermesDeadState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
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
