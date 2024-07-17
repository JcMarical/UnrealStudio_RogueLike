using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using Pathfinding;
using MainPlayer;
using System.Security.Policy;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 所有敌人的基类，所有敌人继承此类
/// </summary>
public class Enemy : MonoBehaviour, IDamageable,ISS
{
    #region 变量声明

    public GameObject player;

    public EnemyFSM enemyFSM;   // 敌人状态机
    public EnemyState patrolState;
    public EnemyState chaseState;
    public EnemyState attackState;
    public EnemyState deadState;

    public Rigidbody2D rb; // 刚体组件
    public Animator anim;  // 动画组件

    public enum EnemyType {melee, ranged}   //敌人类型枚举（近战，远程）
    public enum EnemyQuality {normal, elite, boss}  //敌人品质枚举（普通，精英，Boss）

    [Header("基本数值")]
    public EnemyType enemyType; //敌人类型
    public EnemyQuality enemyQuality;   //敌人品质
    public float maxHealth; //最大生命值
    public float currentHealth; //当前生命值
    public float defense;   //防御力
    public float patrolSpeed;   //巡逻速度
    public float chaseSpeed;    //追击速度
    public float[] speed;   //其他速度
    public float basicPatrolDistance;   //基础巡逻距离
    public float patrolWaitTime;    //巡逻等待时间
    public float hatredTime;    //仇恨时间
    public float[] attackDamage;  //攻击伤害
    public float[] skillDamage;   //技能伤害
    public float[] attackCoolDown;    //攻击冷却时间
    public float[] skillCoolDown;   //技能冷却时间
    public float[] force;    //击退力
    public float[] increasedInjury; //增伤
    public float[] armorPenetration; //破甲状态百分比
    public float scale; //localScale的标准值
    public float speedMultiple; //速度倍数
    public float attackMultiple; //攻击倍数

    [Header("范围检测")]
    public LayerMask playerLayer;   //玩家层
    public LayerMask obstacleLayer; //障碍物层

    public Vector2 attackPoint; //攻击范围检测中心
    public Vector2 visualPoint; //视野范围检测中心
    public float visualRange;    //视野范围
    public float attackRange;   //攻击范围

    [Header("工具类变量")]
    public float globalTimer;   //全局计时器
    public bool isPatrolMove;   //巡逻状态是否移动
    public bool isCollideWall;  //是否撞墙
    public bool isCollidePlayer;    //是否撞到玩家
    public int collideDirection;    //撞到障碍物的方向，1=右，2=上，3=左，4=下
    public bool isAttack;   //是否正在攻击
    public bool isSkill;    //是否正在使用技能
    private bool isInvincible;//判断是否处于无敌状态
    public bool isFixation; //判断是否定身
    public bool isDizzy; //判断是否晕眩

    #endregion

    #region 生命周期

    /// <summary>
    /// Awake生命周期函数，初始化敌人状态机
    /// </summary>
    protected virtual void Awake()
    {
        enemyFSM = new();   // 创建敌人状态机实例

        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();     // 获取刚体组件
        anim = GetComponent<Animator>();   // 获取动画组件
        seeker = GetComponent<Seeker>();   //获取Seeker组件
        speedMultiple = 1;
        attackMultiple = 1;
    }

    /// <summary>
    /// OnEnable生命周期函数，启用敌人时初始化状态机并开始执行第一个状态
    /// </summary>
    protected virtual void OnEnable()
    {
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
        globalTimer = 0;
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
    }

    public void ChaseMove(float speed)
    {
        if (pathPointList != null && pathPointList.Count > 0 && currentIndex >= 0 && currentIndex < pathPointList.Count)
        {
            Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //沿路径点方向
            transform.Translate(direction * speed * Time.deltaTime);
            Flip();
        }
        //Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //沿路径点方向
        //transform.Translate(direction * chaseSpeed * Time.deltaTime);
        //Flip();
    }

    #endregion

    #region 异常状态
    public void SS_Hot(float harm)//炎热 参数代表伤害
    {
        if (!isInvincible)
        {
            currentHealth -= harm;
        }
    }

    public void SS_Freeze(float percent)//寒冷
    {
        if(!isInvincible)
        {
            attackMultiple = (1 - percent);
        }
        //以下为寒冷状态恢复时代码
        //attackMultiple=1;
    }

    public void SS_Fixation()//定身 
    {
        if(!isInvincible)
        {
            isFixation = true;
        }
        //以下为定身状态恢复时代码
        //isFixation=false;
    }

    public void SS_Confuse()//混淆
    {
        //混淆状态无法对敌人产生，写在此处便于接口
    }

    public void SS_Sticky(float percent)//粘滞 参数代表人物速度减少比例
    {
        if(!isInvincible)
        {
            speedMultiple = (1 - percent);
        }
        //以下为定身状态恢复时代码
        //speedMultiple = 1;
    }

    public void SS_Burn(float harm)//燃烧 参数代表伤害
    {
        if(!isInvincible)
        {
            currentHealth-= harm;
        }
        //以下为燃烧状态恢复时代码
    }

    public void SS_Clog(float percent)//阻塞 参数代表人物速度减少比例
    {
        if(!isInvincible)
        {
            speedMultiple= (1 - percent);
        }
        //以下为阻塞状态恢复时代码
        //speedMultiple = 1;
    }

    public void SS_Dizzy()//晕眩
    {
        if(!isInvincible)
        {
            isDizzy = true;
        }
        //以下为晕眩状态恢复时代码
        //isDizzy=false;
    }

    public void SS_Hurry(float percent)//急步 参数代表人物速度增加比例
    {
        if(!isInvincible)
        {
            speedMultiple = 1 + percent;
        }
        //以下为定身状态恢复时代码
        //speedMultiple = 1;
    }

    public void SS_Blind(float radius)//致盲 参数为生成圆的半径
    {
        //该状态对敌人不生效，写在此处便于接口
    }

    public void SS_Charm(Transform target, float speed)//魅惑
    {
        //该状态对敌人不生效，写在此处便于接口
    }

    public void SS_Invincible()//无敌
    {
        isInvincible = true;
        //以下为定身状态恢复时代码
        //isInvincible=false;
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackPoint, attackRange);   //画出攻击范围

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + visualPoint, visualRange);   //画出视野范围
    }

    /// <summary>
    /// 转向函数，让怪物x轴朝向始终与速度x分量方向一致
    /// 在移动函数中调用
    /// </summary>
    public void Flip()   //转向
    {
        transform.localScale = rb.velocity.x >= 0 ? new Vector3(scale, scale, scale) : new Vector3(-scale, scale, scale);
    }

    /// <summary>
    /// 基础移动方法，向一个方向以一定速度移动
    /// </summary>
    /// <param name="direction">移动方向</param>
    /// <param name="speed">移动速度</param>
    public void Move(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Flip();
    }

    /// <summary>
    /// 攻击范围检测方法
    /// </summary>
    /// <returns>玩家在攻击范围内为true，否则为false</returns>
    public bool IsPlayerInAttackRange()
    {
        if (Physics2D.OverlapCircle((Vector2)transform.position + attackPoint, attackRange, playerLayer))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 视野范围检测方法
    /// </summary>
    /// <returns>玩家在视野范围内且不被障碍物阻挡为true，否则为false</returns>
    public bool IsPlayerInVisualRange()  //判断玩家是否进入视野范围
    {

        if (Physics2D.OverlapCircle((Vector2)transform.position + visualPoint, visualRange, playerLayer))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 摧毁该敌人
    /// </summary>
    public void DestroyGameObject() => Destroy(gameObject);

    public void GetHit(float damage, float IncreasedInjury)
    {
        currentHealth -= ((damage + IncreasedInjury - armorPenetration[0]) - defense);
    }

    public void Repelled(float force)
    {
        
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
}
