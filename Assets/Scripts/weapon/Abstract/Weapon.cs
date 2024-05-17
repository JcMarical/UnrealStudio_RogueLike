using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //抽象类，规定攻击方法和武器数据结构体
    public WeaponData weaponData;

    //获取武器攻击间隔
    public float GetAttachInterval(){
        return weaponData.AttachInterval;
    }
    //抽象方法，攻击
    public abstract void Attack();
    //委托，需调用的特殊效果
    public delegate void CarrySpecialEffect();
}
