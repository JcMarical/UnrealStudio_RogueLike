using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

/// <summary>
/// 赫尔墨斯羊的追击状态
/// </summary>
public class SheepChaseState : EnemyState
{
    Sheep sheep;

    public SheepChaseState(Enemy enemy, EnemyFSM enemyFSM, Sheep sheep) : base(enemy, enemyFSM)
    {
        this.sheep = sheep;
    }

    public override void OnEnter()
    {
        sheep.currentSpeed = sheep.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        sheep.AutoPath();

        if (sheep.IsPlayerInAttackRange())
            enemyFSM.ChangeState(sheep.attackState);
    }

    public override void PhysicsUpdate()
    {
        sheep.ChaseMove();
    }

    public override void OnExit()
    {
        sheep.moveDirection = Vector2.zero;
        sheep.currentSpeed = 0;
    }
}

/// <summary>
/// 赫尔墨斯羊的攻击状态
/// </summary>
public class SheepAttackState : EnemyState
{
    Sheep sheep;

    float timer;

    public SheepAttackState(Enemy enemy, EnemyFSM enemyFSM, Sheep sheep) : base(enemy, enemyFSM)
    {
        this.sheep = sheep;
    }

    public override void OnEnter()
    {
        sheep.moveDirection = Vector2.zero;
        sheep.currentSpeed = 0;
        timer = sheep.attackCoolDown[0];

        sheep.ChipAttack();
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            enemyFSM.ChangeState(sheep.chaseState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        sheep.moveDirection = Vector2.zero;
        sheep.currentSpeed = 0;
    }
}

/// <summary>
/// 赫尔墨斯羊的死亡状态
/// </summary>
public class SheepDeadState : BasicDeadState
{
    Sheep sheep;

    public SheepDeadState(Enemy enemy, EnemyFSM enemyFSM, Sheep sheep) : base(enemy, enemyFSM)
    {
        this.sheep = sheep;
    }

    public override void OnEnter()
    {
        if (sheep.master != null)
        {
            sheep.master.RemoveSheep(sheep);
            sheep.master.currentHealth -= sheep.master.currentHealth / 20;
            if (sheep.master.currentHealth < sheep.master.currentHealth / 2)
                sheep.master.enemyFSM.ChangeState(sheep.master.lyreShieldState);
        }

        sheep.sheepList?.Remove(sheep.gameObject);
        
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
