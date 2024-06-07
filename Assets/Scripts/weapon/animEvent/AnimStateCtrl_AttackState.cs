using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimStateCtrl_AttackState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        if(StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.segment!=animator.GetInteger("State")){
            animator.SetInteger("State",animator.GetInteger("State")+1);
        }
        else{
            animator.SetInteger("State",1);
        }
    }
}
