using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_SpookyDice", menuName = "Data/ObtainableObjects/Func/SpookyDice", order = 19)]
public class PFunc_SpookyDice : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
        UseProp();
    }

    public override void UseProp()
    {
        base.UseProp();
        //TODO£ºÊÂ¼þ+=Func;
    }

    public override void Finish()
    {
        base.Finish();
    }

    void Func()
    {
        PlayerBuffMonitor.Instance.Anxiety++;
    }
}
