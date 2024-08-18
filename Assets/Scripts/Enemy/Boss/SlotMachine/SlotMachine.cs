using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 老虎机
/// </summary>
public class SlotMachine : Enemy
{
    public EnemyState appleState;   //苹果
    public EnemyState pearState;    //梨子
    public EnemyState grapeState;    //葡萄
    public EnemyState watermelonState;    //西瓜

    private AttackEnemy meleeAttack;
    private AttackEnemy remoteAttack;
    /*青柠不设状态*/

    [Header("老虎机")]
    [Space(16)]
    [Tooltip("葡萄子弹")] public GameObject grape;
    [Tooltip("酸蚀地板")] public GameObject Ground;
    public Vector3 spawnExtents;   // 生成范围的尺寸

    private void OnDrawGizmosSelected()
    {
        // 在 Unity 编辑器中绘制生成范围的边框，使用当前物体的位置作为中心点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnExtents);
    }

    protected override void Awake()
    {
        base.Awake();

        appleState = new SlotMachineAppleState(this, enemyFSM, this);
        pearState = new SlotMachinePearState(this, enemyFSM, this);
        grapeState = new SlotMachineGrapeState(this, enemyFSM, this);
        watermelonState = new SlotMachineWatermelonState(this, enemyFSM, this);
        deadState = new SlotMachineDeadState(this, enemyFSM, this);

        meleeAttack=GetComponentInChildren<AttackEnemy>();
        remoteAttack=grape.GetComponent<AttackEnemy>(); 
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = appleState;
        force = 3000f;
        base.OnEnable();
    }

    /// <summary>
    /// 抽奖
    /// </summary>
    /// <returns>抽到的状态</returns>
    public EnemyState DrawLottery()
    {
        float rng = Random.Range(0,100);

        /// 测试用
        if (enemyFSM.currentState == watermelonState)
        {
            if (rng < 50)
                Debug.Log("apple");
            else
                Debug.Log("grape"); 
        }
        else
        {
            if (rng < 20)
                Debug.Log("apple");
            else if (rng >= 20 && rng < 40)
                Debug.Log("pear");
            else if (rng >= 40 && rng < 60)
                Debug.Log("pear");
            else if (rng >= 60 && rng < 80)
                Debug.Log("watermelon");

            else
            {
                Debug.Log("lemon");
            }
        }
        ///

        ///当老虎机处于葡萄状态时，禁用近程攻击
        ///处于西瓜状态时，禁用所有攻击
        if (enemyFSM.currentState == grapeState)
        {
            meleeAttack.enabled = false;
            remoteAttack.enabled = true;
        }
        if(enemyFSM.currentState==watermelonState)
        {
            meleeAttack.enabled = false;
            remoteAttack.enabled = false;
        }
        else
        {
            meleeAttack.enabled = true;
            remoteAttack.enabled = false;
        }
        ///

        if (enemyFSM.currentState == watermelonState)
        {
            if (rng < 50)
            {
                force = 3000f;
                return appleState;
            }
            else
            {
                force = 500f;
                return grapeState;
            }
        }
        else
        {
            if (rng < 20)
            {
                force = 3000f;
                return appleState;
            }
            else if (rng >= 20 && rng < 40)
            {
                force = 3000f;
                return pearState;
            }
            else if (rng >= 40 && rng < 60)
            {
                force = 3000f;
                return pearState;
            }              
            else if (rng >= 60 && rng < 80)
            {
                force = 0f;
                return watermelonState;
            }
            else
            {
                //抽到了青柠状态
                LimeAttack();
                return enemyFSM.currentState;
            }
        }
    }

    /// <summary>
    /// 青柠状态的生成酸蚀地板
    /// </summary>
    public void LimeAttack()
    {
        //TODO: 生成酸蚀地板
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(false);

            if (spawnPosition != Vector3.zero)
            {

                // 计算当前位置的障碍物生成
                Instantiate(Ground, spawnPosition, Quaternion.identity);
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
            safetyNet--;
        } while (spawnPosition == Vector3.zero && safetyNet > 0);

        return spawnPosition;
    }

    // 检查位置附近是否已经有障碍物生成
    public bool IsPositionUsed(Vector3Int tilemapPosition)
    {
        // 转换 Tilemap 坐标到世界坐标
        Vector3 worldPosition = tilemap.GetCellCenterWorld(tilemapPosition);

        // 检查 Tilemap 位置上是否有 Tile
        TileBase tile = tilemap.GetTile(tilemapPosition);
        if (tile != null)
        {
            // 如果 Tilemap 位置上有 Tile，则认为位置被使用
            return false;
        }

        // 检查是否有障碍物 Collider2D 在该位置
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);
        if (hitCollider != null && hitCollider.CompareTag("Obstacles"))
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// 葡萄状态的远程攻击
    /// </summary>
    public void GrapeAttack()
    {
        GameObject grape = Instantiate(this.grape, transform.position, Quaternion.identity);
        grape.GetComponent<AttackEnemy>().enemy = this;
        grape.GetComponent<BezierCurve>().targetPosition = player.transform.position;
        grape.GetComponent<BezierCurve>().CalculateCurve();
    }
}
