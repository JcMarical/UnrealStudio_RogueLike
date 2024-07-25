using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class BeggarStatePatrol : EnemyState
{
    protected float basicMoveTime;
    protected float currentMoveTime;
    protected float moveTimer;
    protected float waitTimer;
    protected float moveAngle;
    protected Vector2 moveDirection;

    public BeggarStatePatrol(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        moveAngle = Random.Range(0, 360);
        moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;
        basicMoveTime = enemy.basicPatrolDistance / enemy.patrolSpeed;
        currentMoveTime = Random.Range(enemy.basicPatrolDistance - 1, enemy.basicPatrolDistance + 1) / enemy.patrolSpeed;
        moveTimer = currentMoveTime;
        waitTimer = enemy.patrolWaitTime;
    }

    public override void LogicUpdate()
    {
        if (waitTimer >= 0 && !enemy.isPatrolMove)
            waitTimer -= Time.deltaTime;

        if (waitTimer < 0)
            enemy.isPatrolMove = true;

        if (moveTimer > 0 && enemy.isPatrolMove)
            moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            enemy.isPatrolMove = false;

            moveAngle = Random.Range(moveAngle + 120, moveAngle + 240);
            moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;

            if (currentMoveTime > basicMoveTime * 2 || currentMoveTime < basicMoveTime * 0.5f)
                currentMoveTime = basicMoveTime;
            else
                currentMoveTime *= Random.Range(0.75f, 1.5f);
            moveTimer = currentMoveTime;

            waitTimer = enemy.patrolWaitTime;
        }

        if (enemy.isPatrolMove && enemy.isCollideWall)
        {
            enemy.isPatrolMove = false;
            enemy.isCollideWall = false;

            switch (enemy.collideDirection)
            {
                case 1:
                    moveAngle = Random.Range(-60, 60); break;
                case 2:
                    moveAngle = Random.Range(30, 150); break;
                case 3:
                    moveAngle = Random.Range(120, 240); break;
                case 4:
                    moveAngle = Random.Range(210, 330); break;
                default:
                    moveAngle = Random.Range(0, 360); break;
            }
            moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;

            if (currentMoveTime > basicMoveTime * 2 || currentMoveTime < basicMoveTime * 0.5f)
                currentMoveTime = basicMoveTime;
            else
                currentMoveTime *= Random.Range(0.75f, 1.5f);
            moveTimer = currentMoveTime;

            waitTimer = enemy.patrolWaitTime;
        }
    }

    public override void PhysicsUpdate()
    {
        if (enemy.IsPlayerInVisualRange())
            enemyFSM.ChangeState(enemy.chaseState);
        if (enemy.isPatrolMove && (enemy.isRepelled))
            enemy.Move(moveDirection, enemy.patrolSpeed);
    }

    public override void OnExit()
    {

    }
}


/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
/// </summary>
public class BeggarStateChase : EnemyState
{
    private float coolDownTimer;
    private float hatredTimer;
    private Vector2 chaseDirection;
    private Vector2 retreatDirection;

    public BeggarStateChase(Enemy enemy, EnemyFSM enemyFSM, BeggarEnemy beggarEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isMove", true);  //播放跑的动画
        coolDownTimer = enemy.globalTimer;
        hatredTimer = 2;
        chaseDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
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
            enemy.ChaseMove();
        }

    }

    public override void OnExit()
    {
        //enemy.anim.SetBool("isMove", false);
    }
}


/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
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
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
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
