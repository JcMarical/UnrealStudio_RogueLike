using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    public GameObject[] obstacles; // 障碍物列表
    int obstaclesNumber;
    public int minObstaclesNumber;  //障碍物数量下限
    public int maxObstaclesNumber;    // 障碍物数量上限
    public Vector3 spawnExtents;   // 生成范围的尺寸

    private List<Vector3> usedPositions = new List<Vector3>(); // 已使用的生成位置

    private void OnDrawGizmosSelected()
    {
        // 在 Unity 编辑器中绘制生成范围的边框，使用当前物体的位置作为中心点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnExtents);
    }

    // Start is called before the first frame update
    void Start()
    {
        obstaclesNumber = Random.Range(minObstaclesNumber, maxObstaclesNumber);
        GenerateObstacles();
    }

    // 生成障碍物
    void GenerateObstacles()
    {
        for (int i = 0; i < obstaclesNumber / 2; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();

            if (spawnPosition != Vector3.zero)
            {
                int obstacleIndex = Random.Range(0, obstacles.Length);
                GameObject obstacle = obstacles[obstacleIndex];
                Instantiate(obstacle, spawnPosition, Quaternion.identity);
                usedPositions.Add(spawnPosition);

                // 计算对称位置，并生成对称的障碍物
                Vector3 symmetricalPosition = new Vector3(2*transform.position.x-spawnPosition.x, spawnPosition.y, spawnPosition.z);
                Instantiate(obstacle, symmetricalPosition, Quaternion.identity);
                usedPositions.Add(symmetricalPosition);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn position for obstacle " + i);
            }
        }
    }

    // 获取有效的生成位置
    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        int safetyNet = 100; // 防止无限循环

        do
        {
            // 随机选择一个位置
            spawnPosition = new Vector3(
                transform.position.x + Random.Range(-spawnExtents.x / 2f, spawnExtents.x / 2f),
                transform.position.y + Random.Range(-spawnExtents.y / 2f, spawnExtents.y / 2f),
                transform.position.z
            );

            // 检查生成点是否在 y 轴上对称
            if ((int)spawnPosition.x % 2 != 0)
            {
                spawnPosition.x = Mathf.Round(spawnPosition.x); // 调整 y 坐标为最近的偶数或奇数
            }

            // 检查是否与已使用的位置过于接近
            if (IsNearUsedPosition(spawnPosition))
            {
                spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            }

            safetyNet--;
        } while (spawnPosition == Vector3.zero && safetyNet > 0);

        return spawnPosition;
    }

    // 检查位置附近是否已经有障碍物生成
    bool IsNearUsedPosition(Vector3 pos)
    {
        foreach (Vector3 usedPos in usedPositions)
        {
            if (Vector3.Distance(pos, usedPos) < 0.5f)
            {
                return true;
            }
        }
        return false;
    }
}
