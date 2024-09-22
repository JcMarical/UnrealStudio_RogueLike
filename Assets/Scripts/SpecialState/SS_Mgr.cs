using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum SpecialState_Type
{
    SS_Acide=1,
    SS_Blind,
    SS_Burn,
    SS_Charm,
    SS_Clog,
    SS_Cold,
    SS_Confuse,
    SS_Dizzy,
    SS_Fixation,
    SS_Hurry,
    SS_Injury,
    SS_Invincible,
    SS_Sticky
}

public class SS_Mgr : TInstance<SS_Mgr>
{
    [Header("����״̬����")]
    public string DataPath = "Datas/SpecialStates";
    public List<SpecialState> CopyDatas = new List<SpecialState>();

    public List<SS_FSM> Targets;
    public SS_FSM Target;
    public SpecialState_Type State;
    public float Duration;

    [Header("Editor")]
    public GameObject From;
    

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

    public void AddSpecialState(GameObject Target, string StateName, float Duration,GameObject From)
    {
        SS_FSM target;
        if (Target.GetComponent<Enemy>())
        {
            target = Target.GetComponent<EnemySS_FSM>();
            ((EnemySS_FSM)target).AddState(StateName, Duration,From);
        }
        else
        {
            target = Target.GetComponent<PlayerSS_FSM>();
            ((PlayerSS_FSM)target).AddState(StateName, Duration,From);
        }
    }

    public void AddSpecialState(GameObject Target, SpecialState_Type StateType,float Duration,GameObject From)
    {
        SS_FSM target;
        if (Target.GetComponent<Enemy>())
        {
            target = Target.GetComponent<EnemySS_FSM>();
            ((EnemySS_FSM)target).AddState(StateType, Duration,From);
        }
        else
        {
            target = Target.GetComponent<PlayerSS_FSM>();
            ((PlayerSS_FSM)target).AddState(StateType, Duration,From);
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

    public SpecialState EnumtoState(SpecialState_Type EnumType)
    {
        return GetCopyData((int)EnumType);    
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
