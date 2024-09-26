using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapWeapon : Weapon
{
    public GameObject TrapGameobject;
        private void Awake() =>transform.GetChild(0).GetComponent<SpriteRenderer>().sprite=weaponData.sprite;
    public override void Attack(){
        TrapWeaponAttack();
    }
    public abstract void TrapWeaponAttack();
}
