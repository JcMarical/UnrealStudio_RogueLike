using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 老虎机
/// </summary>
public class SlotMachine : Enemy
{
    public EnemyState appleState;   //苹果
    public EnemyState pearState;    //梨子
    public EnemyState grapeState;    //葡萄
    public EnemyState watermelonState;    //西瓜
    /*青柠不设状态*/

    [Header("老虎机")]
    [Space(16)]
    [Tooltip("葡萄子弹")] public GameObject grape;

    protected override void Awake()
    {
        base.Awake();

        appleState = new SlotMachineAppleState(this, enemyFSM, this);
        pearState = new SlotMachinePearState(this, enemyFSM, this);
        grapeState = new SlotMachineGrapeState(this, enemyFSM, this);
        watermelonState = new SlotMachineWatermelonState(this, enemyFSM, this);
        deadState = new SlotMachineDeadState(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = grapeState;

        base.OnEnable();
    }

    /// <summary>
    /// 抽奖
    /// </summary>
    /// <returns>抽到的状态</returns>
    public EnemyState DrawLottery()
    {
        float rng = Random.Range(0, 100);

        if (enemyFSM.currentState == watermelonState)
        {
            if (rng < 50)
                return appleState;
            else
                return grapeState;
        }
        else
        {
            if (rng < 20)
                return appleState;
            else if (rng >= 20 && rng < 40)
                return pearState;
            else if (rng >= 40 && rng < 60)
                return pearState;
            else if (rng >= 60 && rng < 80)
                return watermelonState;
            else
            {
                //抽到了青柠状态
                LimeAttack();
                return enemyFSM.currentState;
            }
        }
    }

    /// <summary>
    /// 青柠状态的生成酸蚀地板
    /// </summary>
    public void LimeAttack()
    {
        //TODO: 生成酸蚀地板
    }

    /// <summary>
    /// 葡萄状态的远程攻击
    /// </summary>
    public void GrapeAttack()
    {
        GameObject grape = Instantiate(this.grape, transform.position, Quaternion.identity);
        grape.GetComponent<AttackAreaEnemy>().enemy = this;
        grape.GetComponent<BesierCurve>().targetPosition = player.transform.position;
    }
}
