using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 近战武器抽象类，索敌伤害结算由EnemySearchAndDamage实现
/// </summary>
public abstract class MeleeWeapon : Weapon
{
    protected void Awake()
    {
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
