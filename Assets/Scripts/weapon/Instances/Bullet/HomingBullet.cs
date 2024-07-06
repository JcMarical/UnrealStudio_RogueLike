using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class HomingBullet : Bullet
{
    private Action OnAttack;
    //给予初速度
    public override void GetInitialVelocity()
    {
        GetComponent<Rigidbody>().velocity = Speed*Direction;
    }

    //击中检测
    protected override void OnCollisionEnter2D(Collision2D other){
        if(other.transform.CompareTag("Enemy")){
            OnAttack();
        }
    }

    private void  LateUpdate() {
        TraceTargetEnemy();        
    }
    public override void SetAttackEvent(Action action)
    {
        OnAttack+=action;
    }
}
