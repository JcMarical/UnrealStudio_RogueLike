using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public override void Attack(){
        weaponData.Range.SetActive(true);
        RangedWeaponAttack();
        weaponData.Range.SetActive(false);
    }
    public abstract void RangedWeaponAttack();
}
