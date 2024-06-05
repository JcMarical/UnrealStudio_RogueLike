using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public override void Attack(Action action){
        weaponData.Range.SetActive(true);
        RangedWeaponAttack(action);
        weaponData.Range.SetActive(false);
    }
    public abstract void RangedWeaponAttack(Action action);
}
