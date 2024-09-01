using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

/// <summary>
/// “老板” 赫尔墨斯
/// </summary>
public class Hermes : Enemy
{
    public EnemyState summonState;  //一阶段
    public EnemyState caduceusState;    //二阶段双蛇杖
    public EnemyState lyreShieldState;  //二阶段里拉琴护盾
    public EnemyState lyreBarrageState; //二阶段里拉琴弹幕

    public CancellationTokenSource soundWaveBarrageCTK;

    [Header("“老板” 赫尔墨斯")]
    [Space(16)]
    [Tooltip("牛")] public GameObject cow;
    [Tooltip("羊")] public GameObject sheep;
    [Tooltip("潮湿地板")] public GameObject ground;
    public List<Cow> cowList;
    public List<Sheep> sheepList;
    [Space(16)]
    public GameObject caduceusAttackArea;
    [Tooltip("护盾")] public HermesShield shield;
    [Tooltip("音波")] public GameObject soundWave;
    public List<HermesSoundWave> soundWaveList;
    [Space(16)]
    public float shieldTimer;

    [Tooltip("生成范围（选择一个小一点的合适的范围）")] public Vector3 spawnExtents;
    private float distance; //移动距离
    private Vector2 lastPosition;

    private void OnDrawGizmosSelected()
    {
        // 在 Unity 编辑器中绘制生成范围的边框，使用当前物体的位置作为中心点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnExtents);
    }

    protected override void Awake()
    {
        base.Awake();

        summonState = new HermesSummonState(this, enemyFSM, this);
        caduceusState = new HermesCaduceusState(this, enemyFSM, this);
        lyreShieldState = new HermesLyreShieldState(this, enemyFSM, this);
        lyreBarrageState = new HermesLyreBarrageState(this, enemyFSM, this);
        deadState = new HermesDeadState(this, enemyFSM, this);

        cowList = new List<Cow>();
        sheepList = new List<Sheep>();
        lastPosition = transform.position;
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = summonState;
        CreateBulletPool(soundWave);

        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
    }

    public new void Update()
    {
        float movement = Vector2.Distance(lastPosition, (Vector2)transform.position);
        distance += movement;
        lastPosition = transform.position;

        if (distance > tileLength)
        {
            CaduceusChangeFloor();
            distance = 0;
        }
    }

    public void SummonCow()
    {
        Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-spawnExtents.x / 2f, spawnExtents.x / 2f),
            UnityEngine.Random.Range(-spawnExtents.y / 2f, spawnExtents.y / 2f),
            0f);
        Vector3 spawnPosition = GetValidSpawnPosition(true, randomOffset);
        if (spawnPosition != Vector3.zero)
        {
            GameObject cow = Instantiate(this.cow, spawnPosition, Quaternion.identity);
            Cow cowCow = cow.GetComponent<Cow>();
            cowCow.master = this;
            cowList.Add(cowCow);
        }
    }

    public void SummonSheep()
    {
        Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-spawnExtents.x / 2f, spawnExtents.x / 2f),
            UnityEngine.Random.Range(-spawnExtents.y / 2f, spawnExtents.y / 2f),
            0f);
        Vector3 spawnPosition = GetValidSpawnPosition(true,randomOffset);
        if (spawnPosition != Vector3.zero)
        {
            GameObject sheep = Instantiate(this.sheep, spawnPosition, Quaternion.identity);
            Sheep sheepSheep = sheep.GetComponent<Sheep>();
            sheepSheep.master = this;
            sheepList.Add(sheepSheep);
        }
    }

    public void RemoveCow(Cow cow) => cowList.Remove(cow);
    public void RemoveSheep(Sheep sheep) => sheepList.Remove(sheep);


    /// <summary>
    /// 召唤牛羊时附带的攻击
    /// </summary>
    [ContextMenu("召唤攻击")]
    public void SummonAttack()
    {
        float angle = Vector2.SignedAngle(Vector2.right, player.transform.position - transform.position);

        for (int i = 0; i < 3; i++)
        {
            GameObject b1 = bulletPoolList[0].CreateBullet(transform.position);
            GameObject b2 = bulletPoolList[0].CreateBullet(transform.position);
            b1.GetComponent<AttackEnemy>().enemy = this;
            b2.GetComponent<AttackEnemy>().enemy = this;
            b1.GetComponent<HermesSoundWave>().Initialize(2 * tileLength, Quaternion.Euler(0, 0, angle + 13.5f + i * 27) * Vector2.right, 10, false);
            b2.GetComponent<HermesSoundWave>().Initialize(2 * tileLength, Quaternion.Euler(0, 0, angle - 13.5f - i * 27) * Vector2.right, 10, false);
        }
    }

    /// <summary>
    /// 追踪音波攻击
    /// </summary>
    [ContextMenu("追踪音波攻击")]
    public void SoundWaveAttack()
    {
        GameObject b = bulletPoolList[0].CreateBullet(transform.position);
        b.GetComponent<AttackEnemy>().enemy = this;
        HermesSoundWave sw = b.GetComponent<HermesSoundWave>();
        sw.Initialize(1.5f * tileLength, Vector2.zero, 10000, true);
        soundWaveList.Add(sw);
    }

    /// <summary>
    /// 音浪弹幕攻击
    /// </summary>
    public void SoundWaveBarrage()
    {
        soundWaveBarrageCTK = new CancellationTokenSource();
        OnSoundWaveBarrage(soundWaveBarrageCTK.Token).Forget();
    }

    private async UniTask OnSoundWaveBarrage(CancellationToken ctk)
    {
        float angle;
        float[] speed1 = new float[] { -0.5f, 0, 0.5f, 0.5f, 0, -0.5f };
        float[] speed2 = new float[] { 0.75f, -0.75f, -0.25f, 0.25f };

        while (true)
        {
            angle = UnityEngine.Random.Range(0, 360);
            for (int i = 0; i < 8; i++)
            {
                GameObject b = bulletPoolList[0].CreateBullet(transform.position);
                b.GetComponent<AttackEnemy>().enemy = this;
                b.GetComponent<HermesSoundWave>().Initialize(3 * tileLength, Quaternion.Euler(0, 0, i * 45 + angle) * Vector2.right, 6, false, 30);
            }
            for (int i = 0; i < 8; i++)
            {
                GameObject b = bulletPoolList[0].CreateBullet(transform.position);
                b.GetComponent<AttackEnemy>().enemy = this;
                b.GetComponent<HermesSoundWave>().Initialize(3 * tileLength, Quaternion.Euler(0, 0, i * 45 + angle) * Vector2.right, 6, false, -30);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(attackCoolDown[1]), cancellationToken: ctk);

            angle = UnityEngine.Random.Range(0, 360);
            for (int i = 0; i < 18; i++)
            {
                GameObject b = bulletPoolList[0].CreateBullet(transform.position);
                b.GetComponent<AttackEnemy>().enemy = this;
                b.GetComponent<HermesSoundWave>().Initialize((speed1[i % 6] + 2) * tileLength, Quaternion.Euler(0, 0, i * 20 + angle) * Vector2.right, 6, false);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(attackCoolDown[1]), cancellationToken: ctk);

            angle = UnityEngine.Random.Range(0, 360);
            for (int i = 0; i < 16; i++)
            {
                GameObject b = bulletPoolList[0].CreateBullet(transform.position);
                b.GetComponent<AttackEnemy>().enemy = this;
                b.GetComponent<HermesSoundWave>().Initialize((speed2[i % 4] + 2) * tileLength, Quaternion.Euler(0, 0, i * 22.5f + angle) * Vector2.right, 6, false);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(attackCoolDown[1]), cancellationToken: ctk);
        }
    }

    /// <summary>
    /// 双蛇杖攻击
    /// </summary>
    public void CaduceusAttack()
    {
        OnCaduceusAttack().Forget();
    }

    private async UniTask OnCaduceusAttack()
    {
        caduceusAttackArea.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, player.transform.position - transform.position));
        isAttack = true;
        anim.SetTrigger("attack");

        await UniTask.Delay(TimeSpan.FromSeconds(attackCoolDown[1]));

        isAttack = false;
    }

    /// <summary>
    /// 双蛇杖魅惑
    /// </summary>
    public void CaduceusCharm()
    {
        OnCaduceusCharm().Forget();
    }

    private async UniTask OnCaduceusCharm()
    {
        isSkill = true;
        currentSpeed = 0;
        moveDirection = Vector2.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(2));

        player.GetComponent<PlayerSS_FSM>()?.AddState("SS_Charm", 3);
        isSkill = false;
        currentSpeed = chaseSpeed;
    }

    /// <summary>
    /// 双蛇杖潮湿地面
    /// </summary>
    public void CaduceusChangeFloor()
    {
        float chance = UnityEngine.Random.value;
        if (chance<=0.25f)
        {
            Vector3 position=GetValidSpawnPosition(false,transform.position);
            if (position!=Vector3.zero)
            {
                Instantiate(ground,position, Quaternion.identity);
            }
        }
    }

    // 获取有效的生成位置（改进后）
    Vector3 GetValidSpawnPosition(bool enemy,Vector3 randomOffset)
    {
        Vector3 spawnPosition = Vector3.zero;
        int safetyNet = 100; // 防止无限循环

        do
        {

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
        // 转换 TileMap 坐标到世界坐标
        Vector3 worldPosition = tilemap.GetCellCenterWorld(tilemapPosition);

        // 检查 TileMap 位置上是否有 Tile
        TileBase tile = tilemap.GetTile(tilemapPosition);
        if (tile != null)
        {
            // 如果 TileMap 位置上有 Tile，则认为位置被使用
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
}
