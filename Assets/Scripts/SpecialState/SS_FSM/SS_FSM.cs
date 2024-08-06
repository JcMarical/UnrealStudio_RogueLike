using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class SS_FSM : MonoBehaviour
{
    [SerializeField]protected List<SpecialState> StatesList = new List<SpecialState>();

    private void Awake()
    {

    }
    public void Start()
    {

    }

    public void Update()
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
            }
        }
    }

    /// <summary>
    /// ���״̬
    /// </summary>
    /// <param name="state">Ҫ��ӵ�״̬</param>
    /// <param name="Duration">״̬����ʱ��</param>
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
                }
            }
        }

        StatesList.Add(state);
        state.StateAwake();
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
    /// �����µ�״̬������Ĭ��ֵ
    /// </summary>
    /// <param name="Name">��״̬����</param>
    /// <returns></returns>
    public SpecialState CreateNewState(string Name)
    {
        SpecialState newState = (SpecialState)ScriptableObject.CreateInstance(System.Type.GetType(Name));
        SpecialState copyData = SS_Mgr.Instance.GetCopyData(Name);
        if (copyData && newState)
        {
            newState.StateName = copyData.StateName;
            newState.StateDescription = copyData.StateDescription;
            newState.Priority = copyData.Priority;
            newState.Sprite = copyData.Sprite;
            newState.Subordinate = copyData.Subordinate;
            return newState;
        }

        return null;
    }

    /// <summary>
    /// �����µ�״̬������Ĭ��ֵ
    /// </summary>
    /// <param name="state">��״̬����</param>
    /// <returns></returns>
    public SpecialState CreateNewState(SpecialState state)
    {
        SpecialState newState = (SpecialState)ScriptableObject.CreateInstance(state.GetType());
        SpecialState copyData = SS_Mgr.Instance.GetCopyData(state.ID);
        if (newState)
        {
            newState.StateName = copyData.StateName;
            newState.StateDescription = copyData.StateDescription;
            newState.Priority = copyData.Priority;
            newState.Sprite = copyData.Sprite;
            newState.Subordinate = copyData.Subordinate;
            return newState;
        }

        return null;
    }
}
