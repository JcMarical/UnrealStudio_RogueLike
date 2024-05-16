using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    /// <summary>
    /// 武器系统所需所有静态字段，使用或挂载到玩家子物体
    /// </summary>
    #region 初始化
    private void Awake() {
        AllWeapon=new List<GameObject>();
        //武器类型初始化
    }
    #endregion
    #region  所有武器预制体引用集合
    private static List<GameObject> AllWeapon;
    public static  List<GameObject>GetWeaponList(){
        return AllWeapon;
    }
    #endregion
    #region 目前范围内敌人引用链表
        public static List<GameObject> EnemiesWithin;//索敌范围内敌人链表
    #endregion
}
