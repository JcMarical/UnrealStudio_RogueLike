using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    /// <summary>
    /// 武器系统所需所有静态字段，使用或挂载到玩家子物体
    /// </summary>
    
    public static List<GameObject> AllWeapon;//所有武器类型
    public static List<GameObject> ChangeableWeapon;//可切换武器
    public static List<GameObject> EnemiesWithin;//索敌范围内敌人链表
    public static GameObject CurrentWeapon;


    //初始化
    private void Awake() {
        AllWeapon=new List<GameObject>();
        ChangeableWeapon=new List<GameObject>();
    }
}
