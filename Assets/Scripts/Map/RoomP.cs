using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        roomScale = transform.localScale;
    }

    // 触发碰撞事件时，切换摄像机的目标到当前房间
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.ChangeTarget(transform);
        }
    }
}
