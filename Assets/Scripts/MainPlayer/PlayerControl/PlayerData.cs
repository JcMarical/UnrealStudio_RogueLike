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
    public SpriteRenderer playerPicture;//玩家图片
    public float playerSpeed;//速度
    public float playerHealth;//生命
    public float playerDenfense;//防御值
    public float maxHealth;//角色最大生命
    public float weight;//玩家重量
    [Space]

    [Header("武器相关")]
    public float playerAttack;//基础攻击值
    public float playerRange;//基础玩家攻击范围
    public float attackSpeed;//基础玩家攻击速度

    [Header("状态相关")]
    public float lucky;//幸运值
    public float unlucky;//不幸值
    public string strange;//玩家异常状态
}


