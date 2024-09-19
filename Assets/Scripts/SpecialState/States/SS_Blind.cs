using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SS_Blind", menuName = "Data/SpecialState/SS_Blind", order = 10)]
public class SS_Blind : SpecialState
{
    public float EffectValue;
    public override void StateAwake()
    {
        base.StateAwake();
        Target.SS_Blind(EffectValue);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit(List<SpecialState> StateList)
    {
        if (targetType == TargetType.Player)
        {
            if (((Player)Target).mask != null)
            {
                ((Player)Target).mask.SetActive(false);
                Destroy(((Player)Target).mask.gameObject);
                ((Player)Target).mask = null;
            }
        }
        base.StateExit(StateList);
    }

    public override void CopyData(SpecialState StandardData)
    {
        base.CopyData(StandardData);
        EffectValue = (StandardData as SS_Blind).EffectValue;
    }
}
