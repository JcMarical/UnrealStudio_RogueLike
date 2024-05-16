using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态机基类
/// </summary>
/// <remarks>继承此类的有：小怪的基础巡逻、追击、攻击、死亡状态，小怪的技能状态（不是所有小怪都有技能状态，所以没有基础状态），Boss的所有状态</remarks>
public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyFSM enemyFSM;

    public EnemyState(Enemy enemy, EnemyFSM enemyFSM)
    {
        this.enemy = enemy;
        this.enemyFSM = enemyFSM;
    }

    public abstract void OnEnter();  //进入状态时触发
    public abstract void LogicUpdate();  //状态的逻辑更新，在Update里调用
    public abstract void PhysicsUpdate();    //状态的物理更新，在FixedUpdate里调用
    public abstract void OnExit();   //退出状态时触发
}
