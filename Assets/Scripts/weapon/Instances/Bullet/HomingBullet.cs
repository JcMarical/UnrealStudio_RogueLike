using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class HomingBullet : Bullet
{
    //击中检测
    protected void OnCollisionEnter2D(Collision2D other){
        if(other.transform.CompareTag("Enemy")){
            OnAttack();
        }
    }

    // private void  LateUpdate() {
    //     TraceTargetEnemy();        
    // }
    // /// <summary>
    // /// 追踪函数，放到lateUpdate
    // /// </summary>
    // public virtual void TraceTargetEnemy(){
    //     Direction=(TargetEnemy.transform.position-gameObject.transform.position).normalized;
    //     float AngleOfZ= Vector3.Cross(Direction,new Vector3(0,0,transform.position.z)).z>0?Vector3.Angle(Direction,new Vector3(0,0,transform.position.z)):-Vector3.Angle(Direction,new Vector3(0,0,transform.position.z));;
    //     transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,0,AngleOfZ),2f);
    // } 
}
