using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_PiggyBank", menuName = "Data/ObtainableObjects/Func/PiggyBank", order = 14)]
public class PFunc_PiggyBank : PropFunc
{
    public Collection_Data me;

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        //Î¯ÍÐ+=func£»
    }

    public override void Finish()
    {
        base.Finish();
    }

    void Func()
    {
        me.Price++;
    }
}
