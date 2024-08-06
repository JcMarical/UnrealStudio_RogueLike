using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerSS_FSM : SS_FSM
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void AddState(string StateName, float Duration)
    {
        SpecialState newState = CreateNewState(SS_Mgr.Instance.GetType(StateName));
        newState.targetType = SpecialState.TargetType.Player;
        base.AddState(newState, Duration);
    }
}
