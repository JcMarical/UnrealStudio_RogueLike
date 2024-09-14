using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RoomP : MonoBehaviour
{
    // 房间的四个门的GameObject引用
    public GameObject[] doorLeft, doorRight, doorUp, doorDown;
    // 房间是否有对应方向的门
    public bool roomLeft, roomRight, roomUp, roomDown;
    // 公开的变量，用于存储房间的缩放大小
    public Vector3 roomScale;
    // 初始化房间，设置门的激活状态
    public Tilemap tilemap;

    public float distance = 1.5f;
    void Start()
    {
        roomScale = transform.localScale;
    }

    // 触发碰撞事件时，切换摄像机的目标到当前房间
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 切换相机目标为当前房间
            CameraController.instance.ChangeTarget(transform);
            // 获取当前房间上的 ObstaclesAndEnemyManager 脚本
            ObstaclesAndEnemyManager obstaclesAndEnemyManager = GetComponentInChildren<ObstaclesAndEnemyManager>();

            if (obstaclesAndEnemyManager != null)
            {
                // 调用生成敌人和障碍物的方法
                obstaclesAndEnemyManager.Generate();
            }
            else
            {
                Debug.LogWarning("No ObstaclesAndEnemyManager found in the room!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform playerTransform = other.transform;
            Vector3 roomCenter = transform.position;

            Vector3 playerPos = playerTransform.position;
            Vector3 direction = playerPos - roomCenter;

            // 判断方向：根据玩家相对中心的 x 和 y 轴的差值进行判断

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0) // 右侧
                {
                    playerTransform.position += new Vector3(distance, 0, 0);
                }
                else if (direction.x < 0) // 左侧
                {
                    playerTransform.position += new Vector3(-distance, 0, 0);
                }
            }
            else
            {
                if (direction.y > 0) // 上方
                {
                    playerTransform.position += new Vector3(0, distance, 0);
                }
                else if (direction.y < 0) // 下方
                {
                    playerTransform.position += new Vector3(0, -distance, 0);
                }
            }
        }
    }
}
