using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerEasy : MonoBehaviour
{
    public float speed = 5f; // 控制玩家移动速度

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = Vector3.zero; // 初始化移动方向
        if (Input.GetKey(KeyCode.UpArrow)) // 如果按下上箭头键
        {
            moveDir += Vector3.up; // 向上移动
        }
        if (Input.GetKey(KeyCode.DownArrow)) // 如果按下下箭头键
        {
            moveDir += Vector3.down; // 向下移动
        }
        if (Input.GetKey(KeyCode.LeftArrow)) // 如果按下左箭头键
        {
            moveDir += Vector3.left; // 向左移动
        }
        if (Input.GetKey(KeyCode.RightArrow)) // 如果按下右箭头键
        {
            moveDir += Vector3.right; // 向右移动
        }

        // 根据输入和速度改变位置
        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }
}