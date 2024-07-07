using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest_Weapon : MonoBehaviour
{
    public WeaponCtrl weaponCtrl;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.J)){
            weaponCtrl.Attack();
        }
        if(Input.GetKeyDown(KeyCode.F)){
            weaponCtrl.ChangeWeapon();
        }
    }
    private void FixedUpdate() {
        transform.position+=new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0)*0.1f;
    }
}
