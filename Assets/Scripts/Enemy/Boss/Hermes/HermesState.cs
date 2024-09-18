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

        hermes.ssFSM.AddState("SS_Invincible", 114514);
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

        foreach (Cow cow in hermes.cowList)
            cow.GetHit(114514);

        foreach (Sheep sheep in hermes.sheepList)
            sheep.GetHit(114514);
    }
}

/// <summary>
/// 赫尔墨斯二阶段的双蛇杖状态
/// </summary>
public class HermesCaduceusState : EnemyState
{
    Hermes hermes;

    float attackTimer;
    float skillTimer;

    public HermesCaduceusState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
    }

    public override void OnEnter()
    {
        hermes.currentSpeed = hermes.chaseSpeed;
        hermes.globalTimer = 60;
        attackTimer = hermes.attackCoolDown[1];
        skillTimer = 15;
    }

    public override void LogicUpdate()
    {
        if (hermes.globalTimer > 0)
            hermes.globalTimer -= Time.deltaTime;
        else
            enemyFSM.ChangeState(hermes.lyreShieldState);

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else if (attackTimer <= 0 && hermes.IsPlayerInAttackRange() && !hermes.isSkill)
        {
            hermes.CaduceusAttack();
            attackTimer = hermes.attackCoolDown[1];
        }

        if (skillTimer > 0)
            skillTimer -= Time.deltaTime;
        else if (skillTimer <= 0 && !hermes.isAttack)
        {
            hermes.CaduceusCharm();
            skillTimer = 15;
        }

        hermes.CaduceusChangeFloor();

        if (!hermes.isSkill)
            hermes.AutoPath();
        else
            hermes.moveDirection = Vector2.zero;
    }

    public override void PhysicsUpdate()
    {
        hermes.ChaseMove();
    }

    public override void OnExit()
    {
        hermes.moveDirection = Vector2.zero;
        hermes.currentSpeed = 0;
        hermes.globalTimer = 60;
    }
}

/// <summary>
/// 赫尔墨斯二阶段的里拉琴护盾状态
/// </summary>
public class HermesLyreShieldState : EnemyState
{
    Hermes hermes;

    float attackTimer;

    public HermesLyreShieldState(Enemy enemy, EnemyFSM enemyFSM, Hermes hermes) : base(enemy, enemyFSM)
    {
        this.hermes = hermes;
    }

    public override void OnEnter()
    {
        hermes.ssFSM.AddState("SS_Invincible", 114514);
        hermes.shield.gameObject.SetActive(true);
        hermes.currentSpeed = hermes.chaseSpeed;
        attackTimer = 2;
        hermes.shieldTimer = 30;
    }

    public override void LogicUpdate()
    {
        if (hermes.globalTimer > 0)
            hermes.globalTimer -= Time.deltaTime;
        else
            enemyFSM.ChangeState(hermes.caduceusState);

        if (hermes.shieldTimer > 0)
            hermes.shieldTimer -= Time.deltaTime;
        else
            enemyFSM.ChangeState(hermes.lyreShieldState);

        if (hermes.shield.isBroken)
            enemyFSM.ChangeState(hermes.lyreBarrageState);

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else
        {
            attackTimer = hermes.attackCoolDown[0];
            hermes.SoundWaveAttack();
        }

        hermes.AutoPath();
    }

    public override void PhysicsUpdate()
    {
        hermes.ChaseMove();
    }

    public override void OnExit()
    {
        foreach(HermesSoundWave soundWave in hermes.soundWaveList)
            soundWave.pool.Release(soundWave.gameObject);
        hermes.soundWaveList.Clear();

        hermes.moveDirection = Vector2.zero;
        hermes.currentSpeed = 0;
        hermes.ssFSM.RemoveAllState();
        hermes.shield.gameObject.SetActive(false);
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
        hermes.currentSpeed = 0;
        hermes.moveDirection = Vector2.zero;
        hermes.SoundWaveBarrage();
    }

    public override void LogicUpdate()
    {
        if (hermes.globalTimer > 0)
            hermes.globalTimer -= Time.deltaTime;
        else
            enemyFSM.ChangeState(hermes.caduceusState);

        if (hermes.shieldTimer > 0)
            hermes.shieldTimer -= Time.deltaTime;
        else
            enemyFSM.ChangeState(hermes.lyreShieldState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        hermes.soundWaveBarrageCTK.Cancel();
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
        hermes.coinNumber.multiple = GameManager.Instance.CurrentLayer;

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
