using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    public GameObject Bullet;
    public Vector3 FirePosition;
    public override void Attack(){
        CarrySpecialEffect();
        FireBullet();
    }
    public abstract void CarrySpecialEffect();
    public void FireBullet(){
        Instantiate(Bullet,FirePosition,Quaternion.identity);
    }
}
