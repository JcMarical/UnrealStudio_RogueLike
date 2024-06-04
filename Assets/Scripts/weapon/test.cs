using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Click!");
            StaticData.WeaponSlots[StaticData.CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.GetComponent<Weapon>().Attack();
    }
}
}
