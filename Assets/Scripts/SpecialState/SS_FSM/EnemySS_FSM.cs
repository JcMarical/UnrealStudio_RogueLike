using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySS_FSM : SS_FSM
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddState(SpecialState state)
    {
        state.targetType = SpecialState.TargetType.Enemy;
        base.AddState(state);
    }
}
