using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赫尔墨斯的一阶段召唤状态
/// </summary>
public class HermesSummonState : EnemyState
{
    Hermes hermes;

    float cowTimer;
    float sheepTimer;

    public HermesSummonState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
    }

    public override void OnEnter()
    {
        hermes.currentSpeed = 0;
        hermes.moveDirection = Vector2.zero;
        cowTimer = 3;
        sheepTimer = 3;

        hermes.SummonCow();
        hermes.SummonCow();
        hermes.SummonSheep();
        hermes.SummonSheep();

        //hermes.ssFSM.AddState("Invincible", 114514);
    }

    public override void LogicUpdate()
    {
        if (hermes.cowList.Count < 2)
        {
            if (cowTimer > 0)
                cowTimer -= Time.deltaTime;
            else
            {
                cowTimer = 3;
                hermes.SummonCow();
                hermes.SummonAttack();
            }
        }

        if (hermes.sheepList.Count < 2)
        {
            if (sheepTimer > 0)
                sheepTimer -= Time.deltaTime;
            else
            {
                sheepTimer = 3;
                hermes.SummonSheep();
                hermes.SummonAttack();
            }
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        hermes.currentHealth = hermes.maxHealth;
        hermes.ssFSM.RemoveAllState();
        hermes.globalTimer = 60;
    }
}

/// <summary>
/// 赫尔墨斯二阶段的双蛇杖状态
/// </summary>
public class HermesCaduceusState : EnemyState
{
    Hermes hermes;

    public HermesCaduceusState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
    }

    public override void OnEnter()
    {
        hermes.globalTimer = 60;
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        hermes.globalTimer = 60;
    }
}

/// <summary>
/// 赫尔墨斯二阶段的里拉琴护盾状态
/// </summary>
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
        hermes.moveDirection = Vector2.zero;
        hermes.currentSpeed = 0;
    }
}

/// <summary>
/// 赫尔墨斯二阶段的里拉琴弹幕状态
/// </summary>
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

/// <summary>
/// 赫尔墨斯的死亡状态
/// </summary>
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
