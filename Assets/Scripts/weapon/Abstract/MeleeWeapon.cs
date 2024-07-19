using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 近战武器抽象类
/// </summary>
public abstract class MeleeWeapon : Weapon
{
    private void Start() {
        //为近战武器添加索敌和攻击脚本
        if(GetComponent<EnemySearchAndDamage>()==null){
            transform.AddComponent<EnemySearchAndDamage>();
        }
    }
    public override void Attack(Action action){
        GetComponent<WeaponAnimCtrl>().OnAttack();
        RangedWeaponAttack(action);
    }
    public abstract void RangedWeaponAttack(Action action);
}
