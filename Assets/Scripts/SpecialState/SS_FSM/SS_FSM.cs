using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_FSM : MonoBehaviour
{
    protected List<SpecialState> StatesList = new List<SpecialState>();

    private void Awake()
    {

    }
    void Start()
    {

    }

    public void Update()
    {
        foreach (SpecialState state in StatesList)
        {
            if (state.CheckState())
            {
                state.StateUpdate();
            }
            else
            {
                state.StateExit(StatesList);
            }
        }
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="state">要添加的状态</param>
    public void AddState(SpecialState state,float Duration)
    {
        if (!state) return;
        state.Duration = Duration;
        state.BeginTime = Time.time;
        state.Target = this.gameObject.GetComponent<ISS>();
        SS_Invincible invincible = null ;
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
                }
            }
        }

        StatesList.Add(state);
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
}
