using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : RangedWeapon
{
    private void Start() {
        SpecialEffect_OnAttack.AddListener(()=>{
            FireBullet(3);
        });
    }
}
