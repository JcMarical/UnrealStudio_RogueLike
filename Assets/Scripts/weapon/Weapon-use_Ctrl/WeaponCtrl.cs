using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponCtrl : TInstance<WeaponCtrl>
{
    public bool isAttacking;
    public WeaponData_fac _currentWeaponData_fac;

    private void Start() {
        _currentWeaponData_fac = new WeaponData_fac(StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData);
    }
    public static bool isChangable=true;
    /// <summary>
    /// 供玩家调用，进行攻击，触发一次并充能一次
    /// </summary>
    public void Attack(){
        if(!isAttacking){
            StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Attack();
        }
    }
    /// <summary>
    /// 切换主副武器
    /// </summary>
    public void ChangeWeapon(){
        if(isChangable){
            isChangable=false;
            WeaponChange.ChangeWeapon(Attack);
        }
    }
    /// <summary>
    /// 拾取武器，参数为拾取武器引用和被切换的武器索引（0为主武器，1为副武器）
    /// </summary>
    /// <param name="WeaponInScene"></param>
    /// <param name="ReplaceIndex"></param>
    public void PickWeapon(GameObject WeaponInScene,int ReplaceIndex){
            WeaponChange.PickWeapon(WeaponInScene,ReplaceIndex
            );
    }
    /// <summary>
    /// 获取武器数据实际值引用
    /// </summary>
    /// <returns></returns>
    public WeaponData_fac GetFacWeaponData(){
        return _currentWeaponData_fac;
    }
    /// <summary>
    /// 获取当前武器的详细信息
    /// </summary>
    /// <returns>当前武器详细信息，为WeaponData结构体链表，首元素为主武器，另为副武器</returns>
    public List<WeaponData> GetWeaponData(){
        return StaticData.Instance.hasSecondWeapon?new List<WeaponData>
        {
            StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData,
            StaticData.Instance.GetInActiveWeapon().GetComponent<Weapon>().weaponData
        }:
        new List<WeaponData>{
            StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData
        };
    }
    ///<summary>
    ///武器充能，参数为充能量
    ///</summary>
    public void Charge(int i){
        
    }
    ///<summary>
    ///武器充能,充能量为武器默认值
    ///</summary>
    public void Charge(){
        
    }
    private void Update() {
        //跟随玩家
        transform.localPosition=Vector3.zero;
    }

    public void UpdateAttackSpeed(float speed){
        StaticData.Instance.GetActiveWeapon().GetComponent<Animator>().speed = speed;
    }
    public void SettleSpecialEffect(GameObject Enemy){
        foreach (var effect in StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.specialEffect){
            Enemy.GetComponent<EnemySS_FSM>().AddState(effect.targetType.ToString(),effect.Duration);
        }
    }
    public void UpdateAttackRadius(float radius){
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.Range.radius = radius;
    }
}
