using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贪婪之蛇（头）
/// </summary>
public class GreedySnakeHead : Enemy
{
    [Header("贪婪之蛇")]
    [Tooltip("1为上蛇，2为下蛇")] public int index;
    [Space(16)]
    [Tooltip("另一条蛇")] public GreedySnakeHead theOtherSnake;
    [Tooltip("另一条蛇是否死亡")] public bool isTheOtherDead;
    public List<GreedySnakeBody> bodies;

    protected override void Awake()
    {
        base.Awake();

        patrolState = new GreedySnakePatrolState(this, enemyFSM, this);
        deadState = new GreedySnakeDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = patrolState;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void PositionUpdate()
    {
        foreach (GreedySnakeBody body in bodies)
        {
            body.targetPosition = body.frontBody.transform.position;
        }
    }

    public bool IsObstacleInFront()
    {
        float angle = Vector2.SignedAngle(Vector2.right, moveDirection);
        return Physics2D.OverlapBox(transform.position + Quaternion.Euler(0, 0, angle) * new Vector2(tileLength, 0), new Vector2(tileLength / 2, tileLength / 2), obstacleLayer);
    }
}
