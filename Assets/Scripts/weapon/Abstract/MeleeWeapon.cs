using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 近战武器抽象类
/// </summary>
public abstract class MeleeWeapon : Weapon
{
    public override void Attack(Action action){
        GetComponent<WeaponAnimCtrl>().OnAttack();
        SetActiveCollider();
        RangedWeaponAttack(action);
    }
    public abstract void SetActiveCollider();
    public abstract void RangedWeaponAttack(Action action);
}
