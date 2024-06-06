using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class sword : MeleeWeapon{
    public override void RangedWeaponAttack(Action action)
    {
        Debug.Log("Attack");
        GetComponent<WeaponAnimCtrl>().EndEvent+=()=>{
           transform.GetChild(0).GetComponent<Collider2D>().enabled=false;
        };
    }
    public override void SetActiveCollider()
    {
        transform.GetChild(0).GetComponent<Collider2D>().enabled=true;
    }
}
