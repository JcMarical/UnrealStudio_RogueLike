using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float Speed;//速度大小
    public Vector3 Direction;//速度方向，模为一
    public GameObject TargetEnemy;//追踪弹目标
    public abstract void GetInitialVelocity();//给予初速度
    public abstract void GetTargetEnemy();//给予追踪目标
    protected abstract void OnCollisionEnter2D(Collision2D other);//碰撞（击中）检测
}
