using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //抽象类，规定攻击方法和武器数据结构体
    public WeaponData weaponData;
    //抽象方法，攻击
    public abstract void Attack(Action action);
    //委托，需调用的特殊效果
    public delegate void SpecialEffectForCarrying();
    //攻击范围碰撞箱
    public List<GameObject> AttackRange_Collider;
    ///<summary>
    ///武器充能，参数为充能量
    ///</summary>
    public void Charge(int i){
        if(weaponData.CurrentPower+i<weaponData.MaxPower){
            weaponData.CurrentPower+=i;
        }
        else{
            weaponData.CurrentPower=weaponData.MaxPower;
        }
    }
    ///<summary>
    ///武器充能，充能量为默认值
    ///</summary>
    public void Charge(){
        if(weaponData.CurrentPower+weaponData.DefaultCharge_Value<weaponData.MaxPower){
            weaponData.CurrentPower+=weaponData.DefaultCharge_Value;
        }
        else{
            weaponData.CurrentPower=weaponData.MaxPower;
        }
    }
}
