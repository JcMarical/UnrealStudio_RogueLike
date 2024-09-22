using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimStateCtrl_IdleState : AnimEvent
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        if(animator.GetComponent<Weapon>() != null) {
            WeaponCtrl.Instance.isAttacking= false;
            animator.SetInteger("State",0);
        }
    }
}
