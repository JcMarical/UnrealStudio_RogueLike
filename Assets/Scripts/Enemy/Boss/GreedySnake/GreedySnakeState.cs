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
    private int counter;

    public GreedySnakePatrolState(Enemy enemy, EnemyFSM enemyFSM, GreedySnakeHead greedySnake) : base(enemy, enemyFSM)
    {
        this.greedySnake = greedySnake;
    }

    public override void OnEnter()
    {
        greedySnake.currentSpeed = greedySnake.patrolSpeed;
        greedySnake.PositionUpdate();
        counter = 0;

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

            if (greedySnake.isTheOtherDead && counter == 0)
            {
                greedySnake.currentSpeed += 0.5f;
                counter++;
            }

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
        greedySnake.isDead = true;
        greedySnake.gameObject.layer = 2;
        greedySnake.moveDirection = Vector2.zero;
        greedySnake.currentSpeed = 0;

        if (greedySnake.isTheOtherDead)
        {
            PropDistributor.Instance.WhenEnemyDies(enemy);
            PropDistributor.Instance.DistributeCoin(Random.Range(greedySnake.coinNumber.min, greedySnake.coinNumber.max) * greedySnake.coinNumber.multiple);
            for (int i = 0; i < greedySnake.itemRarity.Length; i++)
                enemy.DropItem(i, enemy.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        }

        greedySnake.theOtherSnake.isTheOtherDead = true;
        greedySnake.DestroySnake();
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
