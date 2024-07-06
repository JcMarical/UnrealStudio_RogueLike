using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    /// <summary>
    /// Sent each frame where another object is within a trigger collider
    /// attached to this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Weapon")){
            Debug.Log("1");
            if(Input.GetKeyDown(KeyCode.F)){
                Debug.Log("2");
                GetComponent<WeaponCtrl>().PickWeapon(other.gameObject,1);
            }
        }
    }
}

