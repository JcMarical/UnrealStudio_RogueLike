using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
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
    public int ID;
    public float BeginTime;
    public float Duration;
    public int Priority;
    public Sprite Sprite;
    public List<SpecialState> Subordinate = new List<SpecialState>();
    public GameObject From;

    virtual public void StateAwake() { BeginTime = Time.time;}
    virtual public void StateUpdate() { }
    virtual public void StateExit(List<SpecialState> StateList)
    {
        Debug.Log("State:" + this.name + "ExitOn:" + Time.time);
        StateList.Remove(this);
    }

    public bool CheckState() 
    {
        return (Time.time - BeginTime) < Duration;
    }

    public float TimeRemind()
    {
        return Duration - (Time.time - BeginTime);
    }

    public virtual void CopyData(SpecialState StandardData)
    {
         StateName = StandardData.StateName;
         StateDescription = StandardData.StateDescription;
         Priority = StandardData.Priority;
         Sprite = StandardData.Sprite;
         Subordinate = StandardData.Subordinate;
    }
}
