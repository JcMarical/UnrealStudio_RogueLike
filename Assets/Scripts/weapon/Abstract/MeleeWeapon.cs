using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 近战武器抽象类
/// </summary>
[RequireComponent(typeof(EnemySearchAndDamage))]
public abstract class MeleeWeapon : Weapon
{
    public override void Attack(Action action){
        GetComponent<WeaponAnimCtrl>().OnAttack();
        RangedWeaponAttack(action);
    }
    public abstract void RangedWeaponAttack(Action action);
}
