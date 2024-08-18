using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapWeapon : Weapon
{
    public GameObject TrapGameobject;
    public override void Attack(){
        TrapWeaponAttack();
    }
    public abstract void TrapWeaponAttack();
}
