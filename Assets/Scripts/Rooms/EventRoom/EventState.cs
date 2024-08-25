using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventState
{
    public abstract void OnEnter();
    public abstract void LogicUpdate();
    public abstract void OnExit();
}

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
