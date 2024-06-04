using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_slot : MonoBehaviour
{
    public GameObject Weapon_InSlot;
    private void Awake() {
        Weapon_InSlot = transform.GetChild(0).gameObject;
    }
}
