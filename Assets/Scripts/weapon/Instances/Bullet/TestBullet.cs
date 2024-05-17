using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : Bullet
{
    //给予初速度
    public override void GetInitialVelocity()
    {
        GetComponent<Rigidbody>().velocity = Speed*Direction;
    }
    //追踪函数
    public override void GetTargetEnemy(){

    }

    //击中检测
    protected override void OnCollisionEnter2D(Collision2D other){
        if(other.transform.CompareTag("Enemy")){

        }
    }
}
