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
    public List<Cow> cowList;
    public List<Sheep> sheepList;
    [Space(16)]
    [Tooltip("音波")] public GameObject soundWave;

    protected override void Awake()
    {
        base.Awake();

        cowList = new List<Cow>();
        sheepList = new List<Sheep>();
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = summonState;

        base.OnEnable();
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
    public void SummonAttack()
    {

    }

    /// <summary>
    /// 追踪音波攻击
    /// </summary>
    public void SoundWaveAttack()
    {

    }

    /// <summary>
    /// 音浪弹幕攻击
    /// </summary>
    public void SoundWaveBarrage()
    {

    }
}
