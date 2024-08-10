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
    public Vector3 FirePosition;//开火方向

    public override void Attack(){
        CarrySpecialEffect();
        FireBullet();

    }
    /// <summary>
    /// 执行特殊效果
    /// </summary>
    public abstract void CarrySpecialEffect();
    /// <summary>
    /// 射击，单发
    /// </summary>
    public void FireBullet(){
        Instantiate(Bullet,FirePosition,Quaternion.identity);
        Bullet.GetComponent<Bullet>().OnAttack+=()=>{
            
        };//传递充能委托，子弹打到敌人时自动调用
    }
}
