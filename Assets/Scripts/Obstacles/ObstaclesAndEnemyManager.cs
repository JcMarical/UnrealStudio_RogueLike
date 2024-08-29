﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Enemy;

public class ObstaclesAndEnemyManager : MonoBehaviour
{
    public GameObject[] obstacles; // 障碍物列表
    int obstaclesNumber;
    public int minObstaclesNumber;  //障碍物数量下限
    public int maxObstaclesNumber;    // 障碍物数量上限
    public Vector3 spawnExtents;   // 生成范围的尺寸

    public GameObject[] Enemies; // 存储敌人预制体的数组
    public float targetHealth; // 目标总生命值
    public float healthTolerance; // 生命值浮动范围
    public int maxAttempts; // 最大尝试次数
    public int maxEliteEnemies; // 精英敌人最大数量

    public Tilemap tilemap;
    //public Vector2Int spawnExtents;
    public List<Vector3Int> usedPositions = new List<Vector3Int>();

    public int DoorNum; //一面墙门的数量
    public Vector3[] crossPositions; // 自定义十字中心点坐标

    private void OnDrawGizmosSelected()
    {
        // 在 Unity 编辑器中绘制生成范围的边框，使用当前物体的位置作为中心点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnExtents);

        for (int i = 0; i < DoorNum; i++)
        {
            Vector3 crossCenter;
            // 计算每个十字的中心点位置
            if (crossPositions != null)
            {
                crossCenter = crossPositions[i];
            }
            else
            {
                crossCenter = Vector3.zero;
            }

            // 绘制十字
            DrawCross(crossCenter,i);
        }
    }

    // 绘制十字的函数
    void DrawCross(Vector3 position,int i)
    {
        Gizmos.color = Color.yellow; // 设置十字颜色

        // 绘制水平线
        Gizmos.DrawLine(position + Vector3.left * (spawnExtents.x / 2 + crossPositions[i].x), position + Vector3.right * (spawnExtents.x / 2 - crossPositions[i].x));
        // 绘制垂直线
        Gizmos.DrawLine(position + Vector3.down * (spawnExtents.y / 2 + crossPositions[i].y), position + Vector3.up * (spawnExtents.y / 2 - crossPositions[i].y));
    }

    // Start is called before the first frame update
    void Start()
    {
        obstaclesNumber = Random.Range(minObstaclesNumber, maxObstaclesNumber);
        GenerateObstacles();
        //wideserch();
        //Transform childTransform = gameObject.transform.GetChild(0); // 假设你想获取第一个子物体
        //// 检查获取的子物体 Transform 是否有效
        //if (childTransform != null)
        //{
        //    // 获取子物体的 GameObject
        //    GameObject childObject = childTransform.gameObject;

        //    // 激活子物体
        //    childObject.SetActive(true);
        //}
        // 延迟一帧，以确保敌人已经完全生成并位于场景中
        Invoke(nameof(CheckCollisionWithObstacles), 0.1f);
        GenerateEnemies();
        //更新网格
        AstarPath.active.Scan();

    }
    void wideserch()
    {

    }


    /// <summary>
    /// 障碍生成器
    /// </summary>
    // 生成障碍物
    void GenerateObstacles()
    {
        for (int i = 0; i < obstaclesNumber / 2; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(false);

            if (spawnPosition != Vector3.zero)
            {
                int obstacleIndex = Random.Range(0, obstacles.Length);
                GameObject obstacle = obstacles[obstacleIndex];

                // 计算当前位置的障碍物生成
                Instantiate(obstacle, spawnPosition, Quaternion.identity);
                usedPositions.Add(tilemap.WorldToCell(spawnPosition));

                // 计算对称位置，并生成对称的障碍物
                Vector3 symmetricalPosition = new Vector3(
                    2 * transform.position.x - spawnPosition.x,
                    spawnPosition.y,
                    spawnPosition.z
                );

                Instantiate(obstacle, symmetricalPosition, Quaternion.identity);
                usedPositions.Add(tilemap.WorldToCell(symmetricalPosition));
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn position for obstacle " + i);
            }
        }
    }

    // 获取有效的生成位置（改进后）
    Vector3 GetValidSpawnPosition(bool enemy)
    {
        Vector3 spawnPosition = Vector3.zero;
        int safetyNet = 100; // 防止无限循环

        do
        {
            // 随机选择一个位置
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnExtents.x / 2f, spawnExtents.x / 2f),
                Random.Range(-spawnExtents.y / 2f, spawnExtents.y / 2f),
                0f
            );

            // 计算所在Tilemap格子的中心位置
            Vector3Int tilemapPosition = tilemap.WorldToCell(transform.position + randomOffset);
            spawnPosition = tilemap.CellToWorld(tilemapPosition) + new Vector3(0.5f, 0.5f, 0f);

            // 检查是否在已使用的位置中
            if (IsPositionUsed(tilemapPosition))
            {
                spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            }

            // 检查Y轴对称位置是否有效
            Vector3 symmetricalPosition = new Vector3(
                2 * transform.position.x - spawnPosition.x,
                spawnPosition.y,
                spawnPosition.z
            );
            Vector3Int symmetricalTilemapPosition = tilemap.WorldToCell(symmetricalPosition);
            if (IsPositionUsed(symmetricalTilemapPosition))
            {
                spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            }

            // 检查位置是否在物体位置周围的 ±1 范围内
            //if (!enemy)
            //{
            //    if (Mathf.Abs(spawnPosition.x - gameObject.transform.position.x) <= 1f || Mathf.Abs(spawnPosition.y - gameObject.transform.position.x) <= 1f)
            //    {
            //        spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            //    }
            //}
            if (!enemy)
            {
                float range = 0.5f; // 设定的范围

                foreach (Vector3 crossCenter in crossPositions)
                {
                    // 检查 spawnPosition 是否在当前十字中心附近
                    if (Mathf.Abs(spawnPosition.x - crossCenter.x) <= range || Mathf.Abs(spawnPosition.y - crossCenter.y) <= range)
                    {
                        spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
                        break; // 找到一个匹配的位置后退出循环
                    }
                }
            }

            safetyNet--;
        } while (spawnPosition == Vector3.zero && safetyNet > 0);

        return spawnPosition;
    }

    // 检查位置附近是否已经有障碍物生成
    bool IsPositionUsed(Vector3Int tilemapPosition)
    {
        return usedPositions.Contains(tilemapPosition);
    }
    /// <summary>
    /// 敌人生成器
    /// </summary>
    void GenerateEnemies()
    {
        float currentHealth = 0.0f;
        int eliteEnemiesCount = 0;
        int rangedEnemiesCount = 0;
        int attempts = 0;

        while ((currentHealth < targetHealth && attempts < maxAttempts) || rangedEnemiesCount == 0)
        {
            GameObject enemyPrefab = Enemies[Random.Range(0, Enemies.Length)];
            Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                float enemyHealth = enemyScript.maxHealth;
                if (enemyScript.enemyQuality == EnemyQuality.elite && eliteEnemiesCount > maxEliteEnemies)
                {
                    break;
                }
                // 检查是否可以添加这个敌人到当前总生命值范围内
                if ((currentHealth + enemyHealth <= targetHealth + healthTolerance) || (enemyScript.enemyType == EnemyType.ranged && rangedEnemiesCount == 0))
                {
                    Vector3 spawnPosition = GetValidSpawnPosition(true);

                    if (spawnPosition != Vector3.zero)
                    {
                        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                        Enemy enemyScriptNew = newEnemy.GetComponent<Enemy>();
                        enemyScriptNew.tilemap = tilemap;
                        //敌人的碰撞体如果有碰撞到tag为Obstacles的物体，摧毁敌人，同时break
                        if (CheckCollisionWithObstacles(newEnemy))
                        {
                            // 如果碰撞到障碍物，则摧毁敌人
                            Destroy(newEnemy);
                        }
                        else
                        {
                            currentHealth += enemyHealth;

                            if (enemyScript.enemyQuality == EnemyQuality.elite)
                            {
                                eliteEnemiesCount++;
                            }
                            if (enemyScript.enemyType == EnemyType.ranged)
                            {
                                rangedEnemiesCount++;
                            }

                            usedPositions.Add(tilemap.WorldToCell(spawnPosition));
                        }


                    }
                }
            }

            attempts++;
        }

        Debug.Log("Total Enemies Spawned: " + (eliteEnemiesCount + rangedEnemiesCount));
        Debug.Log("Total Health: " + currentHealth);
    }

    private bool CheckCollisionWithObstacles(GameObject enemy)
    {
        // 获取敌人的碰撞体
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            // 检查敌人是否碰撞到标记为"Obstacles"的物体
            Collider2D[] colliders = Physics2D.OverlapBoxAll(enemyCollider.bounds.center, enemyCollider.bounds.size, 0f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Obstacles"))
                {
                    return true; // 退出方法
                }
            }
            return false;
        }
        return true;
    }
}