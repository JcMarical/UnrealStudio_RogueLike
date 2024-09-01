using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class SS_FSM : MonoBehaviour
{
    [SerializeField]protected List<SpecialState> StatesList = new List<SpecialState>();
    public event Action WhenStateEnter;
    public event Action WhenStateExit;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {

    }


    public virtual void Update()
    {
        for(int i=0; i < StatesList.Count;i++ )
        {
            if (StatesList[i].CheckState())
            {
                StatesList[i].StateUpdate();
            }
            else
            {
                StatesList[i].StateExit(StatesList);
                WhenStateExit?.Invoke();
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach(var v in StatesList)
            {
                Debug.Log(v);
            }
        }
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="state">要添加的状态</param>
    /// <param name="Duration">状态持续时间</param>
    public void AddState(SpecialState state,float Duration)
    {
        if (!state) return;
        state.Duration = Duration;
        state.BeginTime = Time.time;
        state.Target = this.gameObject.GetComponent<ISS>();
        SS_Invincible invincible = new SS_Invincible() ;
        if (IfStateExist(invincible)) return;
        SpecialState same = IfStateExist(state);
        if (same)
        {
            if (same.TimeRemind() < state.Duration)
            {
                same.StateExit(StatesList);
            }
            else return;
        }

        List<SpecialState> Subordinates = IfSubordinateStatesExist(state);
        if (Subordinates != null)
        {
            if (Subordinates.Count != 0)
            {
                foreach (SpecialState SubState in Subordinates)
                {
                    SubState.StateExit(StatesList);
                    WhenStateExit?.Invoke();
                }
            }
        }

        StatesList.Add(state);
        state.StateAwake();
        WhenStateEnter?.Invoke();
    }

    private SpecialState IfStateExist(SpecialState state)
    {
        foreach (SpecialState State in StatesList)
        {
            if (state.GetType() == State.GetType())
                return State;
        }
        return null;
    }

    private List<SpecialState> IfSubordinateStatesExist(SpecialState state)
    {
        if (state.Subordinate.Count == 0) return null;

        List<SpecialState> States = new List<SpecialState>();
        foreach (SpecialState State in StatesList)
        {
            foreach (SpecialState SubState in state.Subordinate)
            {
                if (SubState.GetType() == State.GetType())
                    States.Add(State);
            }
        }
        return States;
    }

    public void RemoveState(SpecialState state)
    {
        if (!state) return;
        if (IfStateExist(state))
        { 
            state.StateExit(StatesList);
        }
    }

    public void RemoveAllState()
    {
        for (int i=0;i < StatesList.Count ; i++)
        {
            StatesList[i].StateExit(StatesList);
        }
    }

    /// <summary>
    /// 创建新的状态并拷贝默认值
    /// </summary>
    /// <param name="Name">新状态名字</param>
    /// <returns></returns>
    public SpecialState CreateNewState(string Name)
    {
        SpecialState newState = (SpecialState)ScriptableObject.CreateInstance(System.Type.GetType(Name));
        SpecialState copyData = SS_Mgr.Instance.GetCopyData(Name);
        if (copyData && newState)
        {
            newState.CopyData(copyData);
            return newState;
        }

        return null;
    }

    /// <summary>
    /// 创建新的状态并拷贝默认值
    /// </summary>
    /// <param name="state">新状态类型</param>
    /// <returns></returns>
    public SpecialState CreateNewState(SpecialState state)
    {
        SpecialState newState = (SpecialState)ScriptableObject.CreateInstance(state.GetType());
        SpecialState copyData = SS_Mgr.Instance.GetCopyData(state.ID);
        if (newState)
        {
            newState.CopyData(copyData);
            return newState;
        }

        return null;
    }

    public List<SpecialState> GetCurrentState()
    {
        return StatesList;
    }
}
