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
    private void Awake() {
        Debug.Log(transform.childCount);
        Weapon_InSlot=transform.childCount!=0?transform.GetChild(0).gameObject:null;
    }
}
