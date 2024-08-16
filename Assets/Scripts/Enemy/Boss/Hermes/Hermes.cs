using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;

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
    public List<Cow> cowList;
    public List<Sheep> sheepList;
    [Space(16)]
    public GameObject caduceusAttackArea;
    [Tooltip("护盾")] public HermesShield shield;
    [Tooltip("音波")] public GameObject soundWave;
    public List<HermesSoundWave> soundWaveList;
    [Space(16)]
    public float shieldTimer;

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

    public void SummonCow()
    {
        GameObject cow = Instantiate(this.cow, transform.position, Quaternion.identity);
        Cow cowCow = cow.GetComponent<Cow>();
        cowCow.master = this;
        cowList.Add(cowCow);

        //TODO: 改变牛的生成位置
    }

    public void SummonSheep()
    {
        GameObject sheep = Instantiate(this.sheep, transform.position, Quaternion.identity);
        Sheep sheepSheep = sheep.GetComponent<Sheep>();
        sheepSheep.master = this;
        sheepList.Add(sheepSheep);

        //TODO: 改变羊的生成位置
    }

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
        caduceusAttackArea.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, player.transform.position - transform.position));
        isAttack = true;
        currentSpeed = 0;
        moveDirection = Vector2.zero;
        anim.SetTrigger("attack");

        await UniTask.Delay(TimeSpan.FromSeconds(attackCoolDown[1]));

        isAttack = false;
        currentSpeed = chaseSpeed;
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
        //TODO: 走过的地块有25%概率生成潮湿地面
    }
}
