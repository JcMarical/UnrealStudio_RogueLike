using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Weapon_slot : MonoBehaviour
{
    [SerializeField]
    private GameObject Weapon;
    public GameObject Weapon_InSlot{
        get{
            return Weapon;
        }
        set{
            if(value != null){
                if(Weapon!=null){
                    Weapon.transform.parent=null;
                }
                Weapon=value;
                Weapon.transform.parent=gameObject.transform;
                Weapon.transform.localPosition=Vector3.zero;
            }
           else{
            Weapon=null;
           }
        }
    }
    public void DisCardWeapon(){
        Weapon_InSlot.transform.parent=null;
        Weapon_InSlot=null;
    }
    public void ChangeThisWeaponToAnother(){
        int index_another=StaticData.Instance.WeaponSlots[0].GetComponent<Weapon_slot>()==this?1:0;
        StaticData.Instance.WeaponSlots[index_another].GetComponent<Weapon_slot>().Weapon_InSlot=Weapon_InSlot;
        Weapon_InSlot=null;
    }
    private void Awake() {
        Weapon_InSlot=transform.childCount!=0?transform.GetChild(0).gameObject:null;
    }
}
