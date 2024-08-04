using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Enemies; // 存储敌人预制体的数组
    public float targetHealth; // 目标总生命值
    public float healthTolerance; // 生命值浮动范围
    public int maxAttempts; // 最大尝试次数
    public int maxEliteEnemies; // 精英敌人最大数量
    public Vector3 spawnExtents; // 生成范围的尺寸

    private List<Vector3> usedPositions = new List<Vector3>(); // 已经使用的位置列表
    private float minDistanceBetweenEnemies = 1.0f; // 敌人之间的最小间距

    private void OnDrawGizmosSelected()
    {
        // 在 Unity 编辑器中绘制生成范围的边框，使用当前物体的位置作为中心点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnExtents);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateEnemies()
    {
        float currentHealth = 0.0f;
        int eliteEnemiesCount = 0;
        int rangedEnemiesCount = 0;
        int attempts = 0;

        while ((currentHealth < targetHealth && attempts < maxAttempts) || rangedEnemiesCount==0)
        {
            GameObject enemyPrefab = Enemies[Random.Range(0, Enemies.Length)];
            Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                float enemyHealth = enemyScript.maxHealth;
                if(enemyScript.enemyQuality == EnemyQuality.elite && eliteEnemiesCount>maxEliteEnemies)
                {
                    break;
                }
                // 检查是否可以添加这个敌人到当前总生命值范围内
                if ((currentHealth + enemyHealth <= targetHealth + healthTolerance) || (enemyScript.enemyType == EnemyType.ranged && rangedEnemiesCount==0))
                {
                    Vector3 spawnPosition = GetValidSpawnPosition();

                    if (spawnPosition != Vector3.zero)
                    {
                        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                        currentHealth += enemyHealth;

                        if (enemyScript.enemyQuality == EnemyQuality.elite)
                        {
                            eliteEnemiesCount++;
                        }
                        if (enemyScript.enemyType == EnemyType.ranged)
                        {
                            rangedEnemiesCount++;
                        }

                        usedPositions.Add(spawnPosition); // 记录已使用的位置
                        MarkUsedArea(spawnPosition); // 标记已使用的区域
                    }
                }
            }

            attempts++;
        }

        Debug.Log("Total Enemies Spawned: " + (eliteEnemiesCount + rangedEnemiesCount));
        Debug.Log("Total Health: " + currentHealth);
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        int safetyNet = 100; // 安全网，防止无限循环

        do
        {
            spawnPosition = transform.position + new Vector3(Random.Range(-spawnExtents.x / 2f, spawnExtents.x / 2f), Random.Range(-spawnExtents.y / 2f, spawnExtents.y / 2f), spawnExtents.z);

            // 检查是否与障碍物重叠
            Collider[] hitColliders = Physics.OverlapBox(spawnPosition, new Vector3(1, 1, 1), Quaternion.identity);
            bool overlapsObstacle = false;
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Obstacles"))
                {
                    overlapsObstacle = true;
                    break;
                }
            }

            // 检查是否与已使用的位置过于接近
            if (overlapsObstacle || IsNearUsedPosition(spawnPosition))
            {
                spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            }

            safetyNet--;
        } while (spawnPosition == Vector3.zero && safetyNet > 0);

        return spawnPosition;
    }

    bool IsNearUsedPosition(Vector3 pos)
    {
        // 检查位置附近是否已经有敌人生成
        foreach (Vector3 usedPos in usedPositions)
        {
            if (Vector3.Distance(pos, usedPos) < minDistanceBetweenEnemies)
            {
                return true;
            }
        }

        return false;
    }

    void MarkUsedArea(Vector3 center)
    {
        // 标记半径为1的范围内的位置为已使用
        float radius = 1.0f;

        List<Vector3> positionsToRemove = new List<Vector3>();

        foreach (Vector3 pos in usedPositions)
        {
            if (Vector3.Distance(pos, center) <= radius)
            {
                positionsToRemove.Add(pos);
            }
        }

        foreach (Vector3 posToRemove in positionsToRemove)
        {
            usedPositions.Remove(posToRemove);
        }
    }
}
