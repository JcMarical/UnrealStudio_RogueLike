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
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        if(!GetComponentInChildren<EnemySearchAndDamage>()){
            transform.GetChild(0).AddComponent<EnemySearchAndDamage>();
        }
    }   
    public override void Attack(Action action){
        GetComponent<WeaponAnimCtrl>().OnAttack();
        RangedWeaponAttack(action);
    }
    public abstract void RangedWeaponAttack(Action action);
}
