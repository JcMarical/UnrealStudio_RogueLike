using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventState
{
    public abstract void OnEnter();
    public abstract void LogicUpdate();
    public abstract void OnExit();
}

/// <summary>
/// 无辜的羔羊
/// </summary>
public class InnocentLambState : EventState
{
    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}

/// <summary>
/// 铜牌打手
/// </summary>
public class BronzeMedalStrikerState : EventState
{
    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
