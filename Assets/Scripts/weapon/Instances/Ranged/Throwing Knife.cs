using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : RangedWeapon
{
    private void Start() {
        WeaponCtrl.Instance.OnAttack+=Fire;
        WeaponCtrl.Instance.OnChangeWeapon+=change;
    }
    private void Fire() {
        FireBullet(3);
    }
    private void change(){
        WeaponCtrl.Instance.OnAttack-=Fire;
        WeaponCtrl.Instance.OnChangeWeapon-=change;
    }
}
