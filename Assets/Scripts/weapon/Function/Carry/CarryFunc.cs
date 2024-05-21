using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryFunc : MonoBehaviour
{
    public void carryFunc(Weapon_EffectType[] Effects,GameObject Target){
        WeaponData weaponData=StaticData.CurrentWeapon.GetComponent<Weapon>().weaponData;
        foreach(var i in Effects){ 
            switch(i){
                case Weapon_EffectType.Repel:{
                    Repel.RepelEnemy((Target.transform.position-StaticData.CurrentWeapon.transform.position).normalized,weaponData.RepelAbility,Target);
                    break;
                }
            }
        }
    }
}
