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
    public EnemyFSM enemyFSM;

    private Rigidbody2D rb;
    private Animator anim;

    public enum EnemyType {Impact/*撞击*/, Melee/*近战*/, Ranged/*远程*/, Fort/*炮台*/, Boss}

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
    
    protected virtual void Awake()
    {
        enemyFSM = new();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        /*子类中在base.OnEnable()之前为enemyFSM.startState赋值*/

        enemyFSM.InitializeState(enemyFSM.startState);
    }

    protected virtual void OnDisable()
    {
        enemyFSM.currentState.OnExit();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        enemyFSM.currentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        enemyFSM.currentState.PhysicsUpdate();
    }
}
