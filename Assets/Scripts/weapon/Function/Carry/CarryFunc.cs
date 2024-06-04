using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryFunc : MonoBehaviour
{
    public void carryFunc(Weapon_EffectType[] Effects,GameObject Target){
        WeaponData weaponData=StaticData.WeaponSlots[StaticData.CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.GetComponent<Weapon>().weaponData;
        foreach(var i in Effects){ 
            switch(i){
                case Weapon_EffectType.Repel:{
                    Repel.RepelEnemy((Target.transform.position-StaticData.WeaponSlots[StaticData.CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.transform.position).normalized,weaponData.RepelAbility,Target);
                    break;
                }
            }
        }
    }
}
