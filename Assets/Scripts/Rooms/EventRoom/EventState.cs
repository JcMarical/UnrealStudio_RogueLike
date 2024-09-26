using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class EventState : MonoBehaviour
{
    protected EventRoomMgr mgr;

    public EventState(EventRoomMgr mgr)
    {
        this.mgr = mgr;
    }

    public abstract void OnEnter();
    public abstract void LogicUpdate();
    public abstract void OnExit();
}

/// <summary>
/// 无辜的羔羊
/// </summary>
public class InnocentLambState : EventState
{
    List<GameObject> sheepList;

    public InnocentLambState(EventRoomMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        sheepList = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            GameObject sheep = Instantiate(mgr.currentEvent.enemys[0]);
            sheepList.Add(sheep);
            sheep.GetComponent<Sheep>().enemyList = sheepList;

            //TODO: 随机羊的位置
            Vector3 Position;
            Position = GetValidSpawnPosition(true,new(6, 6, 0));
            sheep.transform.position = Position;
        }
    }

    public override void LogicUpdate()
    {
        if (sheepList.Count <= 0)
            mgr.ExitState();
    }

    public override void OnExit()
    {
        float rng = Random.Range(0, 100);
        if (rng < 50)
            mgr.DropCollection(2, true);
        else
            mgr.DropProp(2);
    }

    // 获取有效的生成位置（改进后）
    Vector3 GetValidSpawnPosition(bool enemy, Vector3 randomOffset)
    {
        Vector3 spawnPosition = Vector3.zero;
        int safetyNet = 100; // 防止无限循环

        do
        {

            // 计算所在Tilemap格子的中心位置
            randomOffset.x = Random.Range(-randomOffset.x, randomOffset.x);
            randomOffset.y = Random.Range(-randomOffset.y, randomOffset.y);
            Vector3 Position = mgr.currentRoom.transform.position + randomOffset;
            Position = new Vector3(
                    Mathf.Round(Position.x),
                    Mathf.Round(Position.y),
                    Mathf.Round(Position.z)
                    );
            spawnPosition = Position + new Vector3(0.5f, 0.5f, 0f);

            // 检查是否在已使用的位置中
            if (IsPositionUsed(Position))
            {
                spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            }
            safetyNet--;
        } while (spawnPosition == Vector3.zero && safetyNet > 0);

        return spawnPosition;
    }

    // 检查位置附近是否已经有障碍物生成
    public bool IsPositionUsed(Vector3 position, float checkRadius = 1f)
    {
        // 使用 OverlapCircle 检查半径内的碰撞体
        Collider2D hitCollider = Physics2D.OverlapCircle(position, checkRadius);
        if (hitCollider != null && (hitCollider.CompareTag("Obstacles") || hitCollider.CompareTag("Enemy")))
        {
            return false; // 位置被占用
        }

        return true; // 位置未被占用
    }
}

/// <summary>
/// 铜牌打手
/// </summary>
public class BronzeMedalStrikerState : EventState
{
    List<GameObject> enemyList;

    public BronzeMedalStrikerState(EventRoomMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        enemyList = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            GameObject enemy = Instantiate(mgr.currentEvent.enemys[Random.Range(0, mgr.currentEvent.enemys.Length)]);
            enemyList.Add(enemy);
            enemy.GetComponent<Enemy>().enemyList = enemyList;

            //TODO: 随机怪的位置
            Vector3 Position;
            Position = GetValidSpawnPosition(true, new(6, 6, 0));
            enemy.transform.position = Position;
        }
    }

    public override void LogicUpdate()
    {
        if (enemyList.Count <= 0)
            mgr.ExitState();
    }

    public override void OnExit()
    {
        mgr.DropCollection(mgr.currentEvent.items[0], true);
    }
    // 获取有效的生成位置（改进后）
    Vector3 GetValidSpawnPosition(bool enemy, Vector3 randomOffset)
    {
        Vector3 spawnPosition = Vector3.zero;
        int safetyNet = 100; // 防止无限循环

        do
        {

            // 计算所在Tilemap格子的中心位置
            randomOffset.x=Random.Range(-randomOffset.x, randomOffset.x);
            randomOffset.y = Random.Range(-randomOffset.y, randomOffset.y);
            Vector3 Position = mgr.currentRoom.transform.position + randomOffset;
            Position = new Vector3(
                    Mathf.Round(Position.x),
                    Mathf.Round(Position.y),
                    Mathf.Round(Position.z)
                    );
            spawnPosition = Position + new Vector3(0.5f, 0.5f, 0f);

            // 检查是否在已使用的位置中
            if (IsPositionUsed(Position))
            {
                spawnPosition = Vector3.zero; // 重设为零向量，表示无效位置
            }
            safetyNet--;
        } while (spawnPosition == Vector3.zero && safetyNet > 0);

        return spawnPosition;
    }

    // 检查位置附近是否已经有障碍物生成
    public bool IsPositionUsed(Vector3 position, float checkRadius = 1f)
    {
        // 使用 OverlapCircle 检查半径内的碰撞体
        Collider2D hitCollider = Physics2D.OverlapCircle(position, checkRadius);
        if (hitCollider != null && (hitCollider.CompareTag("Obstacles") || hitCollider.CompareTag("Enemy")))
        {
            return false; // 位置被占用
        }

        return true; // 位置未被占用
    }
}
