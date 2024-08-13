using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// “老板” 赫尔墨斯
/// </summary>
public class Hermes : Enemy
{
    public EnemyState summonState;  //一阶段
    public EnemyState caduceusState;    //二阶段双蛇杖
    public EnemyState lyreShieldState;  //二阶段里拉琴护盾
    public EnemyState lyreBarrageState; //二阶段里拉琴弹幕

    [Header("“老板” 赫尔墨斯")]
    [Space(16)]
    [Tooltip("牛")] public GameObject cow;
    [Tooltip("羊")] public GameObject sheep;
    [Tooltip("音波")] public GameObject soundWave;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = summonState;

        base.OnEnable();
    }
}
