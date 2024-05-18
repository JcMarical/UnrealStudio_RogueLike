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
    public float playerSpeed;//速度
    public float playerDamage;//伤害
    public float playerHealth;//生命
    public float playerDenfense;//防御值
    [Space]
    public float maxHealth;//角色最大生命
}
