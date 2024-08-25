using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 子弹基类
/// </summary>
[RequireComponent(typeof(EnemySearchAndDamage),typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public abstract class Bullet : MonoBehaviour
{
    public int BulletKind;//0穿透，1非穿透
    public float speed;
    public Vector2 Direction=new Vector2();//速度方向和大小
    public GameObject TargetEnemy;//追踪弹目标
    public Action OnAttack;
    private float Mileage=0;//累计里程
    public float MaxRange;//最大攻击范围,按格为单位
    private float maxRange=>MaxRange*ConstField.Instance.LengthPerCeil;
    public Rigidbody2D rb;
    public void SetVelocity(Vector2 dir){
            Direction=dir*speed;
            rb.velocity = Direction;
            transform.eulerAngles=new Vector3(0,0,GetAngle_Range360(dir,Vector2.right));
    } 
    protected void Update(){
        Mileage+=rb.velocity.magnitude;
        if(Mileage>maxRange){
            Destroy(gameObject);
        }
    }
    float GetAngle_Range360(Vector3 a,Vector3 b){
        if(Vector3.Cross(a,b).z<0){
            return Vector3.Angle(a,b);
        }
        else return -Vector3.Angle(a,b);
    }
}
