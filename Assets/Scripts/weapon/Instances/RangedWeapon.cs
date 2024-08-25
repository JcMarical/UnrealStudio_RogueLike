using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 远程武器抽象类
/// </summary>
public class RangedWeapon : Weapon
{
    public GameObject Bullet;//装填子弹

    public override void Attack(){
        SpecialEffect_OnAttack.Invoke(); 
        FireBullet();
    }
    /// <summary>
    /// 执行特殊效果
    /// </summary>
    /// <summary>
    /// 射击，单发
    /// </summary>
    public void FireBullet(){
        Vector3 temp=Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position);
        Vector2 Vec=new Vector2(temp.x,temp.y);
        GameObject bullet=Instantiate(Bullet,transform.position,Quaternion.identity);
        bullet.GetComponent<Bullet>().SetVelocity(Vec);
        bullet.GetComponent<Bullet>().OnAttack+=()=>{
        };//传递充能委托，子弹打到敌人时自动调用
    }
    public void FireBullet(int count){
        Vector2 dir=Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position);
        Vector2 normal=new Vector2(-dir.y,dir.x);
        for(int i=0;i<count;i++){
            if(i!=count/2){
                Instantiate(Bullet,new Vector2(transform.position.x,transform.position.y)+normal*0.5f*math.abs(i-count/2),Quaternion.identity).GetComponent<Bullet>().SetVelocity(dir);
            }
        }
        
        Bullet.GetComponent<Bullet>().OnAttack+=()=>{
        };//传递充能委托，子弹打到敌人时自动调用
    }
}
