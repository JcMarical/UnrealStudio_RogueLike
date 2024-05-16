using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// ���е��˵Ļ��࣬���е��˼̳д���
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyFSM enemyFSM;

    private Rigidbody2D rb;
    private Animator anim;

    public enum EnemyType {Impact/*ײ��*/, Melee/*��ս*/, Ranged/*Զ��*/, Fort/*��̨*/, Boss}

    [Header("������ֵ")]
    public EnemyType enemyType; //��������
    public float maxHealth; //�������ֵ
    public float currentHealth; //��ǰ����ֵ
    public float defense;   //������
    public float patrolSpeed;   //Ѳ���ٶ�
    public float chaseSpeed;    //׷������ٶ�
    public float[] attackDamage;  //�����˺�
    public float[] skillDamage;   //�����˺�
    public float[] attackCoolDown;    //������ȴʱ��
    public float[] skillCoolDown;   //������ȴʱ��
    public float chaseRange;    //׷����Χ
    public float attackRange;   //������Χ
    
    protected virtual void Awake()
    {
        enemyFSM = new();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        /*��������base.OnEnable()֮ǰΪenemyFSM.startState��ֵ*/

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
