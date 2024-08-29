using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Chess", menuName = "Data/ObtainableObjects/Func/Chess", order = 12)]
public class PFunc_Chess : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
        UseProp();
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.RealAttackSpeed += 0.2f;
        Player.Instance.RealPlayerSpeed += 0.1f;
    }

    public override void Finish()
    {
        base.Finish();
    }
}
