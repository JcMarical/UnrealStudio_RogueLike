using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Mgr : TInstance<SS_Mgr>
{
    [Header("����״̬����")]
    public string DataPath = "Datas/SpecialStates";
    public List<SpecialState> CopyDatas = new List<SpecialState>();

    public List<GameObject> Targets;
    public GameObject Target;
    public string State;
    public float Duration;

    protected override void Awake()
    {
        base.Awake();
        if (CopyDatas.Count == 0)
        {
            LoadSpecialStates(DataPath);
        }
    }

    private void LoadSpecialStates(string path)
    {
        SpecialState[] states = Resources.LoadAll<SpecialState>(path);
        foreach (SpecialState state in states)
        {
            if (state != null)
            {
                CopyDatas.Add(state);
                Debug.Log($"Loaded special state: {state.name}");
            }
        }
    }

    public void AddSpecialState(GameObject Target,string StateName,float Duration)
    {
        SS_FSM target;
        if (Target.GetComponent<Enemy>())
        {
            target = Target.GetComponent<EnemySS_FSM>();
            ((EnemySS_FSM)target).AddState(StateName, Duration);
        }
        else
        { 
            target = Target.GetComponent<PlayerSS_FSM>();
            ((PlayerSS_FSM)target).AddState(StateName, Duration);
        }
    }

    public void RemoveSpecialState(GameObject Target,SpecialState State)
    {
        SS_FSM target = Target.GetComponent<SS_FSM>();
        if (target)
        {
            target.RemoveState(State);
        }
    }

    public void RemoveAllSpecialState(GameObject Target)
    {
        SS_FSM target = Target.GetComponent<SS_FSM>();
        if (target)
        {
            target.RemoveAllState();
        }
    }

    public SpecialState GetCopyData(string name)
    {
        return CopyDatas.Find(x => x.GetType().ToString() == name);
    }

    public SpecialState GetCopyData(int ID)
    {
        return CopyDatas.Find(x => x.ID == ID);
    }

    public SpecialState GetType(int ID)
    { 
        return GetCopyData(ID);
    }

    public SpecialState GetType(string Name) 
    {
        return GetCopyData(Name);
    }
}
