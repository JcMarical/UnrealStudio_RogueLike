using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;
using System;

/// <summary>
/// 所有敌人的基类，所有敌人继承此类
/// </summary>
public class Enemy : MonoBehaviour, IDamageable, ISS
{
    #region 成员变量声明

    public GameObject player;
    public float force;//对玩家攻击的力

    public EnemyFSM enemyFSM;   // 敌人状态机
    public EnemySS_FSM ssFSM;   // 异常状态状态机
    public EnemyState patrolState;
    public EnemyState chaseState;
    public EnemyState attackState;
    public EnemyState deadState;

    public Rigidbody2D rb; // 刚体组件
    public Animator anim;  // 动画组件
    private SpriteRenderer spriteRenderer;
    public List<EnemyBulletPool> bulletPoolList;

    public enum EnemyType {melee, ranged, both, special}   //敌人类型枚举（近战，远程，近战&远程，特殊）
    public enum EnemyQuality {normal, elite, boss}  //敌人品质枚举（普通，精英，Boss）

    public enum EnemyMutation {none, invisibility, bigger, flash, rampage} //敌人变种枚举（无变种，隐形，巨大化，闪光，狂暴）

    [Serializable]
    public class CoinNumber
    {
        public int min;
        public int max;
        [Tooltip("掉落金币的倍率")] public int multiple = 1;
    }

    public int[] mutationProbability = { 5, 5, 1, 100 };    //变种概率

    public float tileLength = 1;  //Tilemap一格标准长度（未定）

    [Header("基本数值")]
    [Space(16)]
    [Tooltip("敌人类型")] public EnemyType enemyType;
    [Tooltip("敌人品质")] public EnemyQuality enemyQuality;
    [Tooltip("敌人变种")] public EnemyMutation enemyMutation;
    [Tooltip("能否变异")] public bool canMutate = true;
    [Space(16)]
    [Tooltip("最大生命值")] public float maxHealth;
    [Tooltip("当前生命值")] public float currentHealth;
    [Tooltip("能否被击退")] public bool canBeRepelled = true;
    [Space(16)]
    [Tooltip("当前移动方向（不叠加击退）")] public Vector2 moveDirection;
    [Tooltip("巡逻速度")] public float patrolSpeed;
    [Tooltip("追击速度")] public float chaseSpeed;
    [Tooltip("当前速度")] public float currentSpeed;
    [Tooltip("加速度")] public float acceleration;
    [Tooltip("其他速度")] public float[] otherSpeed;
    [Space(16)]
    [Tooltip("基础巡逻距离")] public float basicPatrolDistance;
    [Tooltip("巡逻等待时间")] public float patrolWaitTime;
    [Tooltip("仇恨时间")] public float hatredTime;
    [Space(16)]
    [Tooltip("攻击伤害")] public float[] attackDamage;
    [Tooltip("攻击冷却时间")] public float[] attackCoolDown;
    [Space(16)]
    [Tooltip("增伤倍率")] public float damageIncrease = 0;
    [Tooltip("速度倍率")] public float speedMultiple = 1;
    [Tooltip("攻击间隔倍率")] public float coolDownMultiple = 1;
    [Tooltip("受到伤害倍率")] public float getDamageMultiple = 1;
    [Space(16)]
    [Tooltip("localScale的标准值")] public float scale = 1;

    [Header("掉落物")]
    [Space(16)]
    [Tooltip("掉落金币的上下限")] public CoinNumber coinNumber;
    [Tooltip("固定掉落物品的稀有度")] public int[] itemRarity;

    [Header("变种")]
    [Space(16)]
    [Tooltip("变种类型索引")] public int mutationNumber;
    [Space(16)]
    [Tooltip("渐变持续时间")] public float fadeDuration = 0.5f;
    [Tooltip("显形持续时间")] public float visibleDuration = 1.0f;
    [Tooltip("隐形持续时间")] public float invisibleDuration = 5.0f;

    [Header("范围检测")]
    [Space(16)]
    [Tooltip("玩家层")] public LayerMask playerLayer = 1 << 6;
    [Tooltip("障碍物层")] public LayerMask obstacleLayer = 1 << 7;
    [Space(16)]
    [Tooltip("攻击范围检测中心")] public Vector2 attackPoint;
    [Tooltip("视野范围检测中心")] public Vector2 visualPoint;
    [Tooltip("视野范围")] public float visualRange;
    [Tooltip("攻击范围")] public float attackRange;

    [Header("工具类变量")]
    [Space(16)]
    [Tooltip("全局计时器")] public float globalTimer;
    [Space(16)]
    [Tooltip("巡逻状态是否移动")] public bool isPatrolMove;
    [Tooltip("是否撞墙")] public bool isCollideWall;
    [Tooltip("是否撞到玩家")] public bool isCollidePlayer;
    [Tooltip("撞到障碍物的方向，1=右，2=上，3=左，4=下")] public int collideDirection;
    [Space(16)]
    [Tooltip("是否正在攻击")] public bool isAttack;
    [Tooltip("是否正在使用技能")] public bool isSkill;
    [Space(16)]
    [Tooltip("是否处于无敌状态")] public bool isInvincible;
    [Tooltip("是否定身")] public bool isFixation;
    [Tooltip("是否晕眩")] public bool isDizzy;
    [Tooltip("是否被击退")] public bool isRepelled;
    [Tooltip("重置击退")] public bool repelledBack;
    [Tooltip("是否死亡")] public bool isDead;
    [Space(16)]
    [Tooltip("狂暴概率")] public int rampage;
    [Tooltip("是否狂暴")] public bool isRampage;
    [Tooltip("受击动画时长")] public float attackedTime=0f;
    [Space(16)]
    public List<GameObject> enemyList;  //事件房有用

    private float timer;  //隐身计时器
    private bool isVisible = true;  //是否隐身
    private Color initialColor;

    public Tilemap tilemap;

    #endregion

    #region 成员属性

    /// <summary>
    /// 结算速度倍率，得到最终值
    /// 移动逻辑中调用速度总是调用此属性，不要调用currentSpeed变量
    /// </summary>
    public float CurrentSpeed
    {
        get => isFixation || isDizzy ? 0 : currentSpeed * speedMultiple * tileLength;
        set => currentSpeed = value;
    }

    #endregion

    #region 生命周期

    /// <summary>
    /// Awake生命周期函数，初始化敌人状态机
    /// </summary>
    protected virtual void Awake()
    {
        enemyFSM = new EnemyFSM();   // 创建敌人状态机实例
        ssFSM = GetComponent<EnemySS_FSM>();

        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();     // 获取刚体组件
        anim = GetComponent<Animator>();   // 获取动画组件
        seeker = GetComponent<Seeker>();   //获取Seeker组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        rampage = 20;
        initialColor = spriteRenderer.color;

        enemyList = null;
    }

    /// <summary>
    /// OnEnable生命周期函数，启用敌人时初始化状态机并开始执行第一个状态
    /// </summary>
    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;

        /*子类中在base.OnEnable()之前为enemyFSM.startState赋值*/
        enemyFSM.InitializeState(enemyFSM.startState);  // 初始化敌人状态机并开始执行第一个状态
    }

    /// <summary>
    /// OnDisable生命周期函数，禁用敌人时执行当前状态的OnExit函数
    /// </summary>
    protected virtual void OnDisable()
    {
        enemyFSM.currentState.OnExit(); // 执行当前状态的OnExit函数
    }

    protected virtual void Start()
    {
        if (canMutate)
        {
            mutationNumber = 0;
            int q;
            for (mutationNumber = 0; mutationNumber < mutationProbability.Length; mutationNumber++)
            {
                q = UnityEngine.Random.Range(0, 100);
                if (q < mutationProbability[mutationNumber])
                {
                    break;
                }
            }
            
            timer = visibleDuration; // 开始时物体可见

            switch (mutationNumber)
            {
                case 0:
                    enemyMutation = EnemyMutation.invisibility;
                    break;
                case 1:
                    Bigger();
                    enemyMutation = EnemyMutation.bigger;
                    break;
                case 2:
                    Flash();
                    enemyMutation = EnemyMutation.flash;
                    break;
                case 3:
                    enemyMutation = EnemyMutation.rampage;
                    break;
            }
        }
        else
        {
            mutationNumber = -1;
        }
        transform.localScale = Vector3.one * scale;
    }

    /// <summary>
    /// FixedUpdate生命周期函数，每个固定帧执行当前状态机状态的PhysicsUpdate函数
    /// </summary>
    protected virtual void FixedUpdate()
    {
        enemyFSM.currentState.PhysicsUpdate(); // 执行当前状态机状态的PhysicsUpdate函数
    }

    /// <summary>
    /// Update生命周期函数，每帧执行当前状态机状态的LogicUpdate函数
    /// </summary>
    protected virtual void Update()
    {
        enemyFSM.currentState.LogicUpdate();   // 执行当前状态机状态的LogicUpdate函数
        if (attackedTime>=0f)
        {
            attackedTime -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.color = initialColor;
        }
        switch(mutationNumber)
        {
            case 0:
                Invisibility();
                break; 
            case 3:
                Rampage();
                break;
            default:
                break;
        }
    }

    #endregion

    #region 自动寻路

    private Seeker seeker;
    private List<Vector3> pathPointList;
    private int currentIndex = 0;
    private float pathFindingTime = 0.5f;
    private float pathFindingTimer = 0f;

    private void PathFinding(Vector3 target)  //获取路径点
    {
        currentIndex = 0;
        //三个参数：起点，终点，回调函数
        seeker.StartPath(transform.position, target, Path =>
        {
            pathPointList = Path.vectorPath;
        });
    }

    public void AutoPath()  //自动寻路
    {
        pathFindingTimer += Time.deltaTime;

        //每0.5s调用一次路径生成函数
        if (pathFindingTimer > pathFindingTime)
        {
            PathFinding(player.transform.position);
            pathFindingTimer = 0f;
        }


        if (pathPointList == null || pathPointList.Count <= 0)  //为空则获取路径点
        {
            PathFinding(player.transform.position);
        }
        else if (Vector2.Distance(transform.position, pathPointList[currentIndex]) <= 0.1f)
        {
            currentIndex += 1;
            if (currentIndex >= pathPointList.Count)
            {
                PathFinding(player.transform.position);
            }
        }

        if (pathPointList != null)
        {
            moveDirection = (pathPointList[currentIndex] - transform.position).normalized;
        }
    }

    public void ChaseMove()
    {
        //currentSpeed = Mathf.MoveTowards(currentSpeed, chaseSpeed, acceleration * Time.deltaTime);

        if (pathPointList != null && pathPointList.Count > 0 && currentIndex >= 0 && currentIndex < pathPointList.Count)
        {
            //Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //沿路径点方向
            //transform.Translate(direction * CurrentSpeed * Time.deltaTime);
            transform.Translate(moveDirection * CurrentSpeed * Time.fixedDeltaTime);
            Flip();
        }
        //Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //沿路径点方向
        //transform.Translate(direction * chaseSpeed * Time.deltaTime);
        //Flip();
    }

    #endregion

    #region 接口方法

    #region 异常状态方法
    public void SS_Acide(float harm)//酸蚀 参数代表伤害
    {
        if (!isInvincible)
            currentHealth -= harm;

        if (currentHealth <= 0)
            enemyFSM.ChangeState(deadState);
    }

    public virtual void SS_Freeze(float percent)//寒冷
    {
        if (!isInvincible)
        {
            coolDownMultiple *= 1 + percent;
        }
        //以下为寒冷状态恢复时代码
        //attackMultiple /= 1 + percent;
    }

    public virtual void SS_Fixation()//定身 
    {
        if (!isInvincible)
        {
            isFixation = true;
        }
        //以下为定身状态恢复时代码
        //isFixation=false;
    }

    public virtual void SS_Confuse()//混淆
    {
        //混淆状态无法对敌人产生，写在此处便于接口
    }

    public virtual void SS_Sticky(float percent)//粘滞 参数代表人物速度减少比例
    {
        if (!isInvincible)
        {
            speedMultiple *= 1 - percent;
        }
        //以下为定身状态恢复时代码
        //speedMultiple /= 1 - percent;
    }

    public virtual void SS_Burn(float harm)//燃烧 参数代表伤害
    {
        if (!isInvincible)
            currentHealth -= harm;

        if (currentHealth <= 0)
            enemyFSM.ChangeState(deadState);

        //以下为燃烧状态恢复时代码
    }

    public virtual void SS_Clog(float percent)//阻塞 参数代表人物速度减少比例
    {
        if (!isInvincible)
        {
            speedMultiple *= 1 - percent;
        }
        //以下为阻塞状态恢复时代码
        //speedMultiple /= 1 - percent;
    }

    public virtual void SS_Dizzy()//晕眩
    {
        if (!isInvincible)
        {
            isDizzy = true;
        }
        //以下为晕眩状态恢复时代码
        //isDizzy=false;
    }

    public virtual void SS_Hurry(float percent)//急步 参数代表人物速度增加比例
    {
        if (!isInvincible)
        {
            speedMultiple *= 1 + percent;
        }
        //以下为定身状态恢复时代码
        //speedMultiple /= 1 + percent;
    }

    public virtual void SS_Blind(float radius)//致盲 参数为生成圆的半径
    {
        //该状态对敌人不生效，写在此处便于接口
    }

    public virtual void SS_Charm(Transform target, float speed)//魅惑
    {
        //该状态对敌人不生效，写在此处便于接口
    }

    public virtual void SS_Invincible()//无敌
    {
        isInvincible = true;
        //以下为定身状态恢复时代码
        //isInvincible=false;
    }

    public virtual void SS_Injury()  //破甲
    {
        if (!isInvincible)
        {
            getDamageMultiple *= 1.5f;
        }
        //以下为破甲状态恢复时代码
        //getHitMultiple /= 1.5f;
    }
    #endregion

    #region 受伤方法

    public virtual void GetHit(float damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;

        attackedTime = 0.3f;
        //将敌人颜色变成红色
        spriteRenderer.color = Color.red;

        if (currentHealth <= 0)
            enemyFSM.ChangeState(deadState);
    }

    public void Repelled(float force)
    {

    }

    #endregion

    #endregion

    #region 其他成员方法

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackPoint, attackRange * tileLength);   //画出攻击范围

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + visualPoint, visualRange * tileLength);   //画出视野范围
    }

    /// <summary>
    /// 转向函数，让怪物x轴朝向始终与速度x分量方向一致
    /// 在移动函数中调用
    /// </summary>
    public void Flip()
    {
        spriteRenderer.flipX = moveDirection.x < 0;
    }

    /// <summary>
    /// 基础移动方法，调用前记得改速度和方向
    /// </summary>
    public void Move()
    {
        transform.Translate(moveDirection * CurrentSpeed * Time.fixedDeltaTime);
        Flip();
    }

    /// <summary>
    /// 计算异常状态得到攻击间隔最终值
    /// </summary>
    /// <param name="index">攻击间隔数组索引</param>
    /// <returns>攻击间隔最终值</returns>
    public float GetAttackCoolDown(int index) => attackCoolDown[index] * coolDownMultiple;

    /// <summary>
    /// 攻击范围检测方法
    /// </summary>
    /// <returns>玩家在攻击范围内为true，否则为false</returns>
    public bool IsPlayerInAttackRange() => Physics2D.OverlapCircle((Vector2)transform.position + attackPoint, attackRange * tileLength, playerLayer);

    /// <summary>
    /// 视野范围检测方法
    /// </summary>
    /// <returns>玩家在视野范围内true，否则为false</returns>
    public bool IsPlayerInVisualRange() => Physics2D.OverlapCircle((Vector2)transform.position + visualPoint, visualRange * tileLength, playerLayer);

    /// <summary>
    /// 摧毁该敌人
    /// </summary>
    public void DestroyGameObject() => Destroy(gameObject);

    /// <summary>
    /// 创建子弹对象池
    /// </summary>
    /// <param name="bulletPrefabs">子弹预制体</param>
    public void CreateBulletPool(GameObject bulletPrefab)
    {
        GameObject obj = new GameObject(gameObject.name + "BulletPool");
        EnemyBulletPool pool = obj.AddComponent<EnemyBulletPool>();
        pool.bullet = bulletPrefab;
        bulletPoolList.Add(pool);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            isCollideWall = true;

            if (collision.contacts[0].normal.x > 0)
                collideDirection = 1;
            else if (collision.contacts[0].normal.y > 0)
                collideDirection = 2;
            else if (collision.contacts[0].normal.x < 0)
                collideDirection = 3;
            else if (collision.contacts[0].normal.y < 0)
                collideDirection = 4;
            else
                collideDirection = 0;
        }

        if (collision.gameObject.CompareTag("Player"))
            isCollidePlayer = true;
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
            isCollideWall = false;

        if (collision.gameObject.CompareTag("Player"))
            isCollidePlayer = false;
    }

    private void Invisibility()  //隐形
    {
        timer += Time.deltaTime;

        if (isVisible)
        {
            if (timer >= visibleDuration)
            {
                timer = 0;
                isVisible = false;
            }
            else
            {
                float alpha = Mathf.Lerp(0, initialColor.a, timer / fadeDuration);
                SetAlpha(alpha);
            }
        }
        else
        {
            if (timer >= invisibleDuration)
            {
                timer = 0;
                isVisible = true;
            }
            else
            {
                float alpha = Mathf.Lerp(initialColor.a, 0, timer / fadeDuration);
                SetAlpha(alpha);
            }
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void Bigger()
    {
        getDamageMultiple *= 0.9f;
        if (rb != null)
        {
            rb.mass *= 2;
        }
        scale *= 2;
    }

    private void Flash()
    {
        patrolSpeed *= 2;
        chaseSpeed *= 2;
        //闪光
    }

    public void Rampage() //狂暴判定
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0f)
        {
            if (isRepelled && isCollideWall)
            {
                int rampage1 = UnityEngine.Random.Range(0, 100);
                timer = 0.2f;
                if (rampage1 < rampage)
                {
                    isRampage = true;
                    for (int i = 0; i < attackDamage.Length; i++)
                    {
                        attackDamage[i] *= 1.5f;
                    }
                    //变红
                    ChangeColor(Color.red);

                    rampage = -10;
                }
                else if (rampage > 0)
                {
                    rampage += 10;
                }
            }
        }
    }

    void ChangeColor(Color newColor)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = newColor;
        }
    }

    //public void InitializedDrops(int num)  //物体掉落
    //{
    //    int q = drops.Length; // 获取掉落物数组的长度
    //    Vector2 center = transform.position; // 圆心位置，假设为敌人的位置
    //    float radius = 0.5f; // 圆的半径

    //    for (int i = 0; i < num; i++)
    //    {
    //        // 计算圆内的随机位置
    //        Vector2 randomOffset = Random.insideUnitCircle.normalized * radius;
    //        Vector2 randomPosition = center + randomOffset;

    //        // 在随机位置初始化掉落物品
    //        int randomIndex = Random.Range(0, q);
    //        Initialization(drops[randomIndex], randomPosition);
    //    }
    //}

    //private void Initialization(GameObject drop, Vector2 position)
    //{
    //    // 在指定位置实例化掉落物品
    //    GameObject droppedItem = Instantiate(drop, position, Quaternion.identity);
    //}


    #endregion
}
