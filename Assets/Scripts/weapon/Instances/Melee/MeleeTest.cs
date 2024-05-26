using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTest : MeleeWeapon
{
    public override void CarrySpecialEffect(){

    }
    private void Awake() {
        StaticData.OwndWeapon[StaticData.CurrentWeapon_Index]=gameObject;
    }
}