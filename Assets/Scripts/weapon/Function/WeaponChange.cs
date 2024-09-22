using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponChange : MonoBehaviour
{
    /// <summary>
    /// 武器切换  传入为要加入背包的武器，索引为被替换的武器索引，0或1
    /// </summary>
    /// <param name="TargetWeapon"></param>
    /// <param name="TobeChangedWeapon_Index"></param>
    public static void PickWeapon(GameObject TargetWeapon,int TobeChangedWeapon_Index){
        bool isPrimary=TobeChangedWeapon_Index==StaticData.Instance.CurrentWeapon_Index;
        StaticData instance=StaticData.Instance;
        if(instance.WeaponSlots[TobeChangedWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot==null){
            instance.WeaponSlots[TobeChangedWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot=TargetWeapon;
        }
        if(isPrimary){
            instance.GetActiveWeaponSlot().Weapon_InSlot=TargetWeapon;
        }
        else {
            //非活跃的武器更新后失活
            instance.GetInActiveWeaponSlot().Weapon_InSlot.SetActive(true);
            instance.GetInActiveWeaponSlot().Weapon_InSlot=TargetWeapon;
            instance.GetInActiveWeaponSlot().Weapon_InSlot.SetActive(false);
        }
    }
    /// <summary>
    /// 更换主副武器，即更改当前武器索引
    /// </summary>
    public static bool ChangeWeapon(UnityAction action){
        if(StaticData.Instance.GetInActiveWeapon()!=null){

            //删除原有的武器特殊效果
            WeaponCtrl.Instance.OnAttack-=StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Special_EffectOnAttack;
            WeaponCtrl.Instance.OnDamage-=StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Special_EffectOnDamage;
            //武器切换
            StaticData.Instance.GetActiveWeapon().gameObject.SetActive(false);
            StaticData.Instance.CurrentWeapon_Index=StaticData.Instance.CurrentWeapon_Index==0?1:0;
            StaticData.Instance.GetActiveWeapon().gameObject.SetActive(true);
            WeaponCtrl.isChangable=true;
            //加上新的武器效果
            WeaponCtrl.Instance.OnAttack+=StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Special_EffectOnAttack;
            WeaponCtrl.Instance.OnDamage+=StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Special_EffectOnDamage;

            //调用一次攻击
            AnimStateCtrl_AttackState.AttackStart.AddListener(()=>{
                action?.Invoke();
                AnimStateCtrl_AttackState.AttackStart.RemoveAllListeners();
            });
            // action.Invoke();
            // AnimStateCtrl_AttackState.AttackEnd.AddListener(change);
            return true;
        }
        else{
            return false;
        }
    }
}
