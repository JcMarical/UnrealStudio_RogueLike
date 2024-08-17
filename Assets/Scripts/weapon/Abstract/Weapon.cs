using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器基类
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    //抽象类，规定攻击方法和武器数据结构体
    public WeaponData weaponData;
    //抽象方法，攻击
    public abstract void Attack();
}
