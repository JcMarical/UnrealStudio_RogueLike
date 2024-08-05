using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贪婪之蛇（身体）
/// </summary>
public class GreedySnakeBody : Enemy
{
    public GreedySnakeHead head;    //蛇头
    public GreedySnakeBody frontBody;   //前方的一节身体

    public override void GetHit(float damage)
    {
        
    }
}
