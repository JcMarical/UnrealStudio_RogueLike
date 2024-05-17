using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePYP : Enemy
{
    public CircleStatePYPPatrolState patrolState;
    public CircleStatePYPChaseState chaseState;
    
    private void OnDrawGizmosSelected()
    {
        //Vector2 currentPosition = (Vector2)transform.position;

        //Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(currentPosition + new Vector2(-2, 2), new Vector3(1, 1, 1)); // 左上

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(currentPosition + new Vector2(2, 2), new Vector3(1, 1, 1)); // 右上

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireCube(currentPosition + new Vector2(-2, -2), new Vector3(1, 1, 1)); // 左下

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(currentPosition + new Vector2(2, -2), new Vector3(1, 1, 1)); // 右下
    }

    protected override void Awake()
    {
        base.Awake();

        patrolState = new(this, enemyFSM, this);
        chaseState = new(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = chaseState;

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

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
