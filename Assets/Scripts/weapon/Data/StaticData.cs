using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StaticData : MonoBehaviour
{
    /// <summary>
    /// 武器系统所需所有静态字段，使用或挂载到玩家子物体
    /// </summary>
    int Weapon_Account;
    public static List<GameObject> AllWeapon;//所有武器类型
    public List<GameObject> AllWeapon_Temp;//临时文件，用于初始化
    public static List<GameObject> EnemiesWithin;//索敌范围内敌人链表
    public static int CurrentWeapon_Index;//当前武器
    public static GameObject[] OwndWeapon=new GameObject[2];//拥有的武器

    //初始化
    private void Awake() {
        EnemiesWithin=new List<GameObject>();
        AllWeapon=new List<GameObject>();
        AllWeapon=AllWeapon_Temp;
    }
}
