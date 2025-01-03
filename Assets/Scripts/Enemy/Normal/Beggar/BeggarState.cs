﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 乞丐的基础巡逻状态
/// </summary>
public class BeggarStatePatrol : BasicPatrolState
{
    BeggarEnemy beggarEnemy;

    public BeggarStatePatrol(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {
        this.beggarEnemy = beggarEnemy;
    }

    public override void OnEnter()
    {
        enemy.anim.SetTrigger("idle");
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        if (enemy.IsPlayerInVisualRange() && !enemy.isPatrolMove)
        {
            enemyFSM.ChangeState(enemy.chaseState);
            return;
        }

        base.LogicUpdate();
        if(enemy.isPatrolMove)
        {
            enemy.anim.SetBool("walk", true);
        }
        else
        {
            enemy.anim.SetBool("walk", false);
            enemy.anim.SetTrigger("idle");
        }
    }

    public override void PhysicsUpdate()
    {
        if (!enemy.isRepelled)
        {
            base.PhysicsUpdate();
        }

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


/// <summary>
/// 乞丐的基础追击状态
/// </summary>
public class BeggarStateChase : EnemyState
{
    private float coolDownTimer;
    private float hatredTimer;
    private Vector2 chaseDirection;
    private Vector2 retreatDirection;

    Vector2 direction;
    public BeggarStateChase(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isMove", true);  //播放跑的动画
        coolDownTimer = enemy.globalTimer;
        hatredTimer = 2;
        chaseDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
        enemy.anim.SetBool("walk", true);
    }

    public override void LogicUpdate()
    {
        enemy.AutoPath();
        if (!enemy.IsPlayerInVisualRange())
        {
            hatredTimer -= Time.deltaTime;
        }
        else
        {
            hatredTimer = 5;
        }

        if (hatredTimer <= 0)
        {
            enemyFSM.ChangeState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!enemy.isRepelled)
        {
            enemy.currentSpeed =enemy.chaseSpeed;
            enemy.ChaseMove();
            if(!ReferenceEquals(enemy.transform,null))
            {
                enemy.moveDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
            }
        }
        else
        {
            enemy.currentSpeed = 0;
            enemy.moveDirection = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        enemy.currentSpeed = 0;
        enemy.moveDirection = Vector2.zero;
        //enemy.anim.SetBool("isMove", false);
        enemy.anim.SetBool("walk", false);
    }
}


/// <summary>
/// 乞丐的基础攻击状态
/// </summary>
public class BeggarStateAttack : EnemyState
{
    public BeggarStateAttack(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        //丢失仇恨切换到巡逻状态的逻辑判断
        if (!enemy.IsPlayerInAttackRange())
        {
            enemyFSM.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// 小怪的基础死亡状态
/// </summary>
public class BeggarStateDead : EnemyState
{
    public BeggarStateDead(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isDead", true);
        enemy.gameObject.layer = 2;
        //int dropsNum = Random.Range(0, enemy.drops.Length);
        //if (enemy.drops.Length != 0)
        //{
        //    enemy.InitializedDrops(Random.Range(enemy.dropsNumber[2 * dropsNum], enemy.dropsNumber[2 * dropsNum + 1]));
        //}
        enemy.anim.SetBool("walk", false);
        enemy.anim.SetBool("dead", true);
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        enemy.rb.velocity = Vector2.zero;
    }

    public override void OnExit()
    {

    }
}
