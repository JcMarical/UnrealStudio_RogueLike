using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贪婪之蛇的巡逻状态
/// </summary>
public class GreedySnakePatrolState : EnemyState
{
    GreedySnakeHead greedySnake;

    private float timer;
    private float turnProbability = 0;
    private float rng;

    public GreedySnakePatrolState(Enemy enemy, EnemyFSM enemyFSM, GreedySnakeHead greedySnake) : base(enemy, enemyFSM)
    {
        this.greedySnake = greedySnake;
    }

    public override void OnEnter()
    {
        greedySnake.currentSpeed = greedySnake.patrolSpeed;
        greedySnake.PositionUpdate();

        switch(greedySnake.index)
        {
            case 1:
                greedySnake.moveDirection = Vector2.right;
                break;
            case 2:
                greedySnake.moveDirection = Vector2.left;
                break;
            default:
                break;
        }

        timer = greedySnake.tileLength / greedySnake.currentSpeed;
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            greedySnake.PositionUpdate();

            turnProbability += 0.05f;
            rng = Random.Range(0f, 1f);
            if (rng <= turnProbability || greedySnake.IsObstacleInFront())
            {
                rng = Random.Range(0f, 1f);
                if (greedySnake.moveDirection.y == 0)
                {
                    Vector2 dir = greedySnake.moveDirection;

                    if (rng < 0.5f)
                        greedySnake.moveDirection = Vector2.up;
                    else
                        greedySnake.moveDirection = Vector2.down;

                    if (greedySnake.IsObstacleInFront())
                    {
                        greedySnake.moveDirection *= -1;
                        if (greedySnake.IsObstacleInFront())
                            greedySnake.moveDirection = -dir;
                    }
                }
                    
                else if(greedySnake.moveDirection.x == 0)
                {
                    Vector2 dir = greedySnake.moveDirection;

                    if (rng < 0.5f)
                        greedySnake.moveDirection = Vector2.right;
                    else
                        greedySnake.moveDirection = Vector2.left;

                    if (greedySnake.IsObstacleInFront())
                    {
                        greedySnake.moveDirection *= -1;
                        if (greedySnake.IsObstacleInFront())
                            greedySnake.moveDirection = -dir;
                    }
                }

                turnProbability = 0;
            }

            if (greedySnake.isTheOtherDead)
                greedySnake.currentSpeed += 0.5f;

            timer = greedySnake.tileLength / greedySnake.currentSpeed;
        }
    }

    public override void PhysicsUpdate()
    {
        greedySnake.Move();
    }

    public override void OnExit()
    {
        
    }
}

/// <summary>
/// 贪婪之蛇的死亡状态
/// </summary>
public class GreedySnakeDeadState : BasicDeadState
{
    GreedySnakeHead greedySnake;

    public GreedySnakeDeadState(Enemy enemy, EnemyFSM enemyFSM, GreedySnakeHead greedySnake) : base(enemy, enemyFSM)
    {
        this.greedySnake = greedySnake;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        greedySnake.theOtherSnake.isTheOtherDead = true;
        greedySnake.DestroyParent();
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
