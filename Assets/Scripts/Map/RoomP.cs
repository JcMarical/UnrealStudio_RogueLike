using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public GameObject BackGround;

    public float distance = 3f;
    public bool isPlayerIn;
    private bool isFinish=false;
    private bool isOpen=false;
    List<GameObject> enabledChildren = new List<GameObject>();

    public Vector2 doorSize = new Vector2(0.5f, 0.5f);
    public bool initial;

    private ObstaclesAndEnemyManager obstaclesAndEnemyManager;

    void Start()
    {
        roomScale = transform.localScale;
        if (initial )
        {
            isPlayerIn = true;
        }
    }
    //public List<GameObject> GetEnabledChildren(Transform parent)
    //{

    //    foreach (Transform child in parent)
    //    {
    //        if (child.gameObject.activeSelf)
    //        {
    //            enabledChildren.Add(child.gameObject);
    //        }
    //    }

    //    return enabledChildren;
    //}

    //public void DisableEnabledChildren()
    //{
    //    foreach (GameObject child in enabledChildren)
    //    {
    //        child.SetActive(false);
    //    }
    //}

    //public void EnableEnabledChildren()
    //{
    //    foreach (GameObject child in enabledChildren)
    //    {
    //        child.SetActive(true);
    //    }
    //}

    void Update()
    {
        if (isPlayerIn)
        {
            BackGround.SetActive(false);
            Bounds bounds = GetComponent<Collider2D>().bounds;

            //所有碰撞体
            Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);
            bool foundEnemy = false;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    foundEnemy = true;
                    break;
                }
            }

            if (!foundEnemy)
            {
                isFinish = true;
            }
        }
        else
        {
            BackGround.SetActive(true);
        }

        if (isFinish && !isOpen)
        {
            UnlockRoomDoors();
        }
    }

    private void UnlockRoomDoors()
    {
        // 将四个列表合并到一个数组中
        GameObject[][] allDoors = { doorLeft, doorRight, doorUp, doorDown };

        foreach (GameObject[] doors in allDoors)
        {
            foreach (GameObject door in doors)
            {
                Collider2D collider = door.GetComponent<Collider2D>();
                Rigidbody2D doorRigidbody2D = door.GetComponent<Rigidbody2D>();
                if (collider != null)
                {
                    Destroy(collider);
                }
                if (doorRigidbody2D != null)
                {
                    Destroy(doorRigidbody2D);
                }
            }
        }
    }
    private void LockAllDoors()
    {
        // 将四个列表合并到一个数组中
        GameObject[][] allDoors = { doorLeft, doorRight, doorUp, doorDown };

        foreach (GameObject[] doors in allDoors)
        {
            foreach (GameObject door in doors)
            {
                Collider2D doorCollider = door.GetComponent<Collider2D>();
                if (doorCollider == null)
                {
                    doorCollider = door.AddComponent<BoxCollider2D>();
                    doorCollider.isTrigger = false;
                    // 设置碰撞体的尺寸
                    ((BoxCollider2D)doorCollider).size = doorSize;
                }
                else if (doorCollider is BoxCollider2D)
                {
                    // 如果已存在 BoxCollider2D，则更新尺寸
                    ((BoxCollider2D)doorCollider).size = doorSize;
                }

                Rigidbody2D doorRigidbody2D = door.GetComponent<Rigidbody2D>();
                if (doorRigidbody2D == null)
                {
                    doorRigidbody2D = door.AddComponent<Rigidbody2D>();
                    doorRigidbody2D.gravityScale = 0f;
                    doorRigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
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
            isPlayerIn = true;
            if (obstaclesAndEnemyManager != null)
            {
                // 调用生成敌人和障碍物的方法
                LockAllDoors();
                if (obstaclesAndEnemyManager.boss == true)
                {
                    Debug.Log("youboss");
                    obstaclesAndEnemyManager.GenerateBossEnemies();
                }
                else
                {
                    obstaclesAndEnemyManager.Generate();
                }
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
            isPlayerIn = false;
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
