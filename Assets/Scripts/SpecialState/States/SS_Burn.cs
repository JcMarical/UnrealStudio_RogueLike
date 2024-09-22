using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[CreateAssetMenu(fileName = "SS_Burn", menuName = "Data/SpecialState/SS_Burn", order = 6)]
public class SS_Burn : SpecialState
{
    public float Harm_Player = 0.1f;
    public float Harm_Enemy = 0.1f;

    [SerializeField]private float LastEffectTime = -10;
    public float EffectInterval = 1;    

    public override void StateAwake()
    {
        base.StateAwake();
    }

    public override void StateUpdate()
    {
        Debug.Log(Time.time - LastEffectTime);
        base.StateUpdate();
        if (Time.time - LastEffectTime > EffectInterval)
        {
            Debug.Log("½øÈë");
            if (targetType == TargetType.Player)
            {
                Target.SS_Burn(Harm_Player);
            }
            else
            { 
                Target.SS_Burn(Harm_Enemy);
            }
            LastEffectTime = Time.time;
        }
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        base.StateExit(StateList);
    }

    public override void CopyData(SpecialState StandardData)
    {
        base.CopyData(StandardData);
        Harm_Player = (StandardData as SS_Burn).Harm_Player;
        Harm_Enemy = (StandardData as SS_Burn).Harm_Enemy;
    }
}
