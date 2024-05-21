using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float Speed;//速度大小
    public Vector3 Direction;//速度方向，模为一
    public GameObject TargetEnemy;//追踪弹目标
    public abstract void GetInitialVelocity();//给予初速度

    /// <summary>
    /// 追踪函数，放到lateUpdate
    /// </summary>
    public virtual void TraceTargetEnemy(){
        Direction=(TargetEnemy.transform.position-gameObject.transform.position).normalized;
        float AngleOfZ= Vector3.Cross(Direction,new Vector3(0,0,transform.position.z)).z>0?Vector3.Angle(Direction,new Vector3(0,0,transform.position.z)):-Vector3.Angle(Direction,new Vector3(0,0,transform.position.z));;
        transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,0,AngleOfZ),2f);
    }   
    protected abstract void OnCollisionEnter2D(Collision2D other);//碰撞（击中）检测
}
