using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpecialState : ScriptableObject
{
    public enum TargetType
    {
        Player,
        Enemy
    }
    public TargetType targetType { get; set; }
    public ISS Target;
    public string StateName;
    [TextArea(10,20)]
    public string StateDescription;
    public float BeginTime;
    public float Duration;
    public int Priority;
    public Sprite Sprite;
    public List<SpecialState> Subordinate = new List<SpecialState>();

    virtual public void StateAwake() { BeginTime = Time.time; }
    virtual public void StateUpdate() { }
    virtual public void StateExit(List<SpecialState> StateList) 
    { 
        StateList.Remove(this);
    }

    virtual public bool CheckState() 
    {
        return (Time.time - BeginTime) > Duration;
    }

    virtual public float TimeRemind()
    {
        return Duration - (Time.time - BeginTime);
    }

}
