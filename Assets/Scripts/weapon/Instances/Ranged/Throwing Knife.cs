using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : RangedWeapon
{

    private void Start() {
        Special_EffectOnAttack+=()=>{
            FireBullet();
            FireBullet(3);
        };
    }
}
