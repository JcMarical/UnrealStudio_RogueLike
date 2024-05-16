using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public override void Attack(){
        RangedWeaponAttack();
    }
    public abstract void RangedWeaponAttack();
}
