using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态机基类，所有敌人状态机继承此类
/// </summary>
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
