using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 远程武器抽象类
/// </summary>
public abstract class RangedWeapon : Weapon
{
    public GameObject Bullet;//装填子弹
    public Vector3 FirePosition;//开火位置

    public override void Attack(){
        FireBullet();
    }
    /// <summary>
    /// 执行特殊效果
    /// </summary>
    /// <summary>
    /// 射击，单发
    /// </summary>
    public void FireBullet(){
        Instantiate(Bullet,FirePosition,Quaternion.identity).GetComponent<Bullet>().SetVelocity(Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position));
        Bullet.GetComponent<Bullet>().OnAttack+=()=>{
        };//传递充能委托，子弹打到敌人时自动调用
    }
}
