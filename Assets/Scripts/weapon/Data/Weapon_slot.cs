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
            if(Weapon!=null){
                Weapon.transform.parent=null;
            }
            Weapon=value;
            Weapon.transform.parent=gameObject.transform;
            Weapon.transform.localPosition=Vector3.zero;
        }
    }
}
