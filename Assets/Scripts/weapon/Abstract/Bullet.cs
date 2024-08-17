using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹基类
/// </summary>
[RequireComponent(typeof(EnemySearchAndDamage),typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public abstract class Bullet : MonoBehaviour
{
    public int BulletKind;//0穿透，1非穿透
    public float speed;
    public Vector2 Direction;//速度方向和大小
    public GameObject TargetEnemy;//追踪弹目标
    public Action OnAttack;
    private float Mileage=0;//累计里程
    public float MaxRange;//最大攻击范围,按格为单位
    private float maxRange=>maxRange*ConstField.Instance.LengthPerCeil;
    private Rigidbody2D rb;
    private void Start()=>rb=GetComponent<Rigidbody2D>();
    public virtual void SetVelocity(Vector2 dir){
        Direction=dir*speed;
        rb.velocity = Direction;
    } 
    protected void Update(){
        Mileage+=rb.velocity.magnitude;
        if(Mileage<maxRange){
            Destroy(gameObject);
        }
    }
}
