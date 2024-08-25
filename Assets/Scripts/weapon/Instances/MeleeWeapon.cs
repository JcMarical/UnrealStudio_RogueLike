using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 近战武器抽象类，索敌伤害结算由EnemySearchAndDamage实现
/// </summary>
public class MeleeWeapon : Weapon
{
    protected void Awake()
    {
        weaponData.Range=GetComponentInChildren<CircleCollider2D>();
        //确保第一个子物体，即sprite有索敌和伤害脚本
        if(!GetComponentInChildren<EnemySearchAndDamage>()){
            transform.GetChild(0).AddComponent<EnemySearchAndDamage>();
        }
    }   
    public override void Attack(){
        //攻击动画
        GetComponent<WeaponAnimCtrl>().OnAttack();
        //武器属性更新，充能

        
    }
}
