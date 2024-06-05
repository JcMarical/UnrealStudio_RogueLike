using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapWeapon : Weapon
{
    public override void Attack(Action action){
        TrapWeaponAttack(action);
    }
    public abstract void TrapWeaponAttack(Action action);
}
