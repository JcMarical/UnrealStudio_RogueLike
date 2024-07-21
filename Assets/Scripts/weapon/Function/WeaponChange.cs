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
        bool isPrimary=TobeChangedWeapon_Index==StaticData.Instance.CurrentWeapon_Index?true:false;
        StaticData instance=StaticData.Instance;
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
    public static void ChangeWeapon(UnityAction action){
        StaticData.Instance.ChangeWeapon(action);
    }
}
