using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AnimStateCtrl_AttackState : AnimEvent
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        AttackStart.Invoke();
        if(StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.segment!=animator.GetInteger("State")){
            animator.SetInteger("State",animator.GetInteger("State")+1);
        }
        else{
            animator.SetInteger("State",1);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        AttackEnd.Invoke();
    }
}
