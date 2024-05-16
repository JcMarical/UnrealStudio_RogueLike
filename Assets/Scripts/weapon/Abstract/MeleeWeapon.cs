using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    public override void Attack(){
        MeleeWeaponAttack();
    }
    public abstract void MeleeWeaponAttack();
}
