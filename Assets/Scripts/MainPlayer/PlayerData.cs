using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Properties;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "SOData/MainPlayer")]

///<summary>
///角色相关属性类
///</summary>
public class PlayerData : ScriptableObject
{
    [Header("基础设置")]
    public float playerSpeed;//速度
    public int playerHealth;//生命
    public float playerDenfense;//防御值
    public float maxHealth;//角色最大生命
    [Space]
    [Header("攻击相关")]
    public float attackDistance;//攻击距离
    public float attackInterval;//攻击时间间隔
    [Space]
    [Header("技能相关")]
    public string currentSkill;//当前技能
    public string ReserveSkill;//后备技能
   
}

public class WeaponClass
{
    [Header("武器相关")]
    public GameObject currentWeapon;//当前武器
    public Dictionary<GameObject, int> Weapon;//不同武器以及其对应伤害

}
