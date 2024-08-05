using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贪婪之蛇（身体）
/// </summary>
public class GreedySnakeBody : Enemy
{
    public GreedySnakeHead head;    //蛇头
    public GameObject frontBody;   //前方的一节身体
    public Vector3 targetPosition;

    protected override void Awake()
    {
        
    }

    protected override void OnEnable()
    {
        
    }

    protected override void OnDisable()
    {
        
    }

    protected override void Start()
    {
        currentSpeed = head.currentSpeed;
    }

    protected override void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.fixedDeltaTime);
    }

    protected override void Update()
    {
        
    }

    public override void GetHit(float damage)
    {
        head.currentHealth -= damage * (1 / (Mathf.Log(2, rb.mass) + 1)) * head.getHitMultiple;
    }
}
