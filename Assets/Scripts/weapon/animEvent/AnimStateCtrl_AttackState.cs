using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AnimStateCtrl_AttackState : AnimEvent
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        AttackStart.Invoke();
        WeaponCtrl.Instance.isAttacking = true;
        if(StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.Range)
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.Range.enabled = true;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        AttackEnd.Invoke();
        WeaponCtrl.Instance.isAttacking= false;
        if(StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.Range)
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.Range.enabled = false;
    }
}
