using Cysharp.Threading.Tasks;
using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttackCtrl : StateMachineBehaviour
{
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
        JudgeWeapon(animator).Forget();
    }


    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);
    }

    async UniTaskVoid JudgeWeapon(Animator animator)
    {
        switch (Player.Instance.weaponData.damageKind)
        {
            case DamageKind.TrapWeapon:

                break;

            case DamageKind.MeleeWeapon:
                await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName("EmptyAttack"));
                Player.Instance.playerAnimation.ChangeAnimation("MeleeAttack", 0, 1);
                Player.Instance.speedDown = 0.5f;
                break;

            case DamageKind.RangedWeapon:
                await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName("EmptyAttack"));
                Player.Instance.playerAnimation.ChangeAnimation("Shoot", 0, 1);
                Player.Instance.speedDown = 0f;
                break;

            default: break;
        }
    }


    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
