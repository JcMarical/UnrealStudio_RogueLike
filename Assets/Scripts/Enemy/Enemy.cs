using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 所有敌人的基类，所有敌人继承此类
/// </summary>
public class Enemy : MonoBehaviour
{
    #region 变量声明

    public GameObject Player;

    public EnemyFSM enemyFSM;   // 敌人状态机

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
    public float[] attackDamage;  //攻击伤害
    public float[] skillDamage;   //技能伤害
    public float[] attackCoolDown;    //攻击冷却时间
    public float[] skillCoolDown;   //技能冷却时间
    public float chaseRange;    //追击范围
    public float attackRange;   //攻击范围
    public float scale; //localScale的标准值

    [Header("范围检测")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public bool inChaseRange;
    public bool inAttackRange;

    #endregion

    #region 生命周期

    /// <summary>
    /// Awake生命周期函数，初始化敌人状态机
    /// </summary>
    protected virtual void Awake()
    {
        enemyFSM = new();   // 创建敌人状态机实例

        rb = GetComponent<Rigidbody2D>();  // 获取刚体组件
        anim = GetComponent<Animator>();   // 获取动画组件
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

    /// <summary>
    /// 转向函数，让怪物x轴朝向始终与速度x分量方向一致
    /// 在移动函数中调用
    /// </summary>
    public void Flip()
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
}
