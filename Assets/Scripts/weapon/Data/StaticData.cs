using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 武器数据单例类，用于获得角色携带武器数据
/// </summary>
public class StaticData :W_TInstance<StaticData>
{
    /// <summary>
    /// 武器系统所需所有静态字段，使用或挂载到玩家子物体
    /// </summary>
    public int CurrentWeapon_Index;//当前武器
    public bool hasSecondWeapon=>WeaponSlots[1].GetComponent<Weapon_slot>().Weapon_InSlot!=null;//是否拥有副武器
    public bool hasMainWeapon=>WeaponSlots[0].GetComponent<Weapon_slot>().Weapon_InSlot!=null;
    public GameObject[] WeaponSlots=new GameObject[2];//武器槽位

    //初始化
    private void Start() {
        CurrentWeapon_Index=0;
    }
    ///<summary>
    ///获得当前武器索引
    ///</summary>
    public GameObject GetActiveWeapon(){
        return WeaponSlots[CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot;
    }
    ///<summary>
    ///获得副武器索引
    ///</summary>
    public GameObject GetInActiveWeapon(){
        return WeaponSlots[CurrentWeapon_Index^1].GetComponent<Weapon_slot>().Weapon_InSlot;
    }
    ///<summary>
    ///获得当前武器槽位索引
    ///</summary>
    public Weapon_slot GetActiveWeaponSlot(){
        return WeaponSlots[CurrentWeapon_Index].GetComponent<Weapon_slot>();
    }
    ///<summary>
    ///获得副武器武器槽位索引
    ///</summary>
    public Weapon_slot GetInActiveWeaponSlot(){
        return WeaponSlots[CurrentWeapon_Index^1].GetComponent<Weapon_slot>();
    }
    // private void change(){
    //     WeaponSlots[CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.SetActive(false);
    //     CurrentWeapon_Index=CurrentWeapon_Index==0?1:0;
    //     WeaponSlots[CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.SetActive(true);
    //     WeaponCtrl.isChangable=true;
    //     AnimStateCtrl_AttackState.AttackEnd.RemoveListener(change);
    // }
}
