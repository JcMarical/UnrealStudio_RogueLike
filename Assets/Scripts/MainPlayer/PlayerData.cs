using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Properties;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "SOData/MainPlayer")]

///<summary>
///��ɫ���������
///</summary>

public class PlayerData : ScriptableObject
{
    public float playerSpeed;//�ٶ�
    public float playerDamage;//�˺�
    public float playerHealth;//����
    public float playerDenfense;//����ֵ
    [Space]
    public float maxHealth;//��ɫ�������
}
