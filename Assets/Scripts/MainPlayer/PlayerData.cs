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
    [Space]
    [Header("状态相关")]
    public int lucky;//幸运值
    public int unlucky;//不幸值
   
}


