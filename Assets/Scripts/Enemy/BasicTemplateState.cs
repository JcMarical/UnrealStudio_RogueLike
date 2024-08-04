using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// 此状态只含巡逻移动逻辑，其他逻辑需额外添加
/// </summary>
public class BasicPatrolState : EnemyState
{
    protected float basicMoveTime;
    protected float currentMoveTime;
    protected float moveTimer;
    protected float waitTimer;
    protected float moveAngle;

    public BasicPatrolState(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
    {
        
    }

    public override void OnEnter()
    {
        moveAngle = Random.Range(0, 360);
        enemy.moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;
        enemy.currentSpeed = enemy.patrolSpeed;
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
        {
            enemy.currentSpeed = enemy.patrolSpeed;
            enemy.isPatrolMove = true;
        }

        if (moveTimer > 0 && enemy.isPatrolMove)
            moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            enemy.isPatrolMove = false;
            enemy.currentSpeed = 0;
            enemy.moveDirection = Vector2.zero;

            moveAngle = Random.Range(moveAngle + 120, moveAngle + 240);
            enemy.moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;

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
            enemy.currentSpeed = 0;
            enemy.moveDirection = Vector2.zero;

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
            enemy.moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;

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
        if (enemy.isPatrolMove)
            enemy.Move();
    }

    public override void OnExit()
    {
        enemy.isPatrolMove = false;
        enemy.currentSpeed = 0;
        enemy.moveDirection = Vector2.zero;
    }
}
