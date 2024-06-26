﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using Pathfinding;

/// <summary>
/// 所有敌人的基类，所有敌人继承此类
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
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

    public enum EnemyType { Impact/*撞击*/, Melee/*近战*/, Ranged/*远程*/, Fort/*炮台*/, Boss }  //敌人类型

    [Header("基本数值")]
    public EnemyType enemyType; //敌人类型
    public float maxHealth; //最大生命值
    public float currentHealth; //当前生命值
    public float defense;   //防御力
    public float patrolSpeed;   //巡逻速度
    public float chaseSpeed;    //追击或后撤速度
    public float currentSpeed;  //当前速度
    public float[] attackDamage;  //攻击伤害
    public float[] skillDamage;   //技能伤害
    public float[] attackCoolDown;    //攻击冷却时间
    public float[] skillCoolDown;   //技能冷却时间
    public float[] force;    //击退力
    public float[] increasedInjury; //增伤
    public float[] armorPenetration; //破甲状态百分比
    public float chaseRange;    //追击范围
    public float attackRange;   //攻击范围
    public float scale; //localScale的标准值

    [Header("范围检测")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public Vector2 attackPoint;
    public Vector2 visualPoint;

    public bool inChaseRange;
    public bool inAttackRange;

    [Header("工具类变量")]
    public float globalTimer;
    public bool isAttack;
    public bool isSkill;

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
    /// Update生命周期函数，每帧执行当前状态机状态的LogicUpdate函数
    /// </summary>
    protected virtual void Update()
    {
        enemyFSM.currentState.LogicUpdate();   // 执行当前状态机状态的LogicUpdate函数
    }

    /// <summary>
    /// FixedUpdate生命周期函数，每个固定帧执行当前状态机状态的PhysicsUpdate函数
    /// </summary>
    protected virtual void FixedUpdate()
    {
        enemyFSM.currentState.PhysicsUpdate(); // 执行当前状态机状态的PhysicsUpdate函数
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

    public void ChaseMove()
    {
        Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //沿路径点方向
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
        Flip();
    }

    #endregion

    /// <summary>
    /// 转向函数，让怪物x轴朝向始终与速度x分量方向一致
    /// 在移动函数中调用
    /// </summary>
    public void Flip()   //转向
    {
        transform.localScale = rb.velocity.x >= 0 ? new Vector3(scale, scale, scale) : new Vector3(-scale, scale, scale);
    }

    /// <summary>
    /// 巡逻状态的移动方法
    /// </summary>
    /// <param name="direction">移动方向</param>
    public void PatrolMove(Vector2 direction)
    {
        transform.Translate(direction * patrolSpeed * Time.deltaTime);
        Flip();
    }

    /// <summary>
    /// 追击状态的移动方法
    /// </summary>
    /// <param name="direction">后撤方向</param>
    public void RetreatMove(Vector2 direction)
    {
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
        Flip();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackPoint, attackRange);   //画出攻击范围

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + visualPoint, chaseRange);   //画出视野范围
    }

    /// <summary>
    /// 障碍物检测方法
    /// </summary>
    /// <returns>玩家与敌人之间有障碍物为true，否则为false</returns>
    public bool IsPlayerBehindObstacle()
    {
        Vector2 direction;
        float distance;

        direction = player.transform.position - transform.position;
        distance = direction.magnitude;

        return Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
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

        if (Physics2D.OverlapCircle((Vector2)transform.position + visualPoint, chaseRange, playerLayer) && !IsPlayerBehindObstacle())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 摧毁该敌人
    /// </summary>
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void GetHit(float damage, float IncreasedInjury)
    {
        currentHealth-=((damage + IncreasedInjury - armorPenetration[0]) - defense);
    }

    public void Repelled(float force, string type)
    {
        
    }
}
