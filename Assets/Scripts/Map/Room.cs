using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    // 房间的四个门的GameObject引用
    public GameObject doorLeft, doorRight, doorUp, doorDown;

    // 房间是否有对应方向的门
    public bool roomLeft, roomRight, roomUp, roomDown;

    // 从起点到此房间的步数
    public int stepToStart;

    // 显示步数的文本UI
    public Text text;

    // 房间门的数量
    public int doorNumber;

    // 初始化房间，设置门的激活状态
    void Start()
    {
        doorLeft.SetActive(roomLeft);    // 激活或禁用左门
        doorRight.SetActive(roomRight);  // 激活或禁用右门
        doorUp.SetActive(roomUp);        // 激活或禁用上门
        doorDown.SetActive(roomDown);    // 激活或禁用下门
    }

    // 更新房间状态，包括计算步数和门的数量
    public void UpdateRoom(float xOffset, float yOffset)
    {
        // 计算从起点到此房间的步数
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
        text.text = stepToStart.ToString(); // 在UI中显示步数

        // 根据是否有门来增加门的数量
        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
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
