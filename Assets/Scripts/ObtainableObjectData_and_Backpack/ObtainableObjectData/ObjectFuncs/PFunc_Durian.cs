using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Durian", menuName = "Data/ObtainableObjects/Func/Durian", order = 17)]
public class PFunc_Durian : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
        UseProp();
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.RealPlayerSpeed -= 0.1f;
        Player.Instance.playerData.playerAttack += 0.2f;
    }

    public override void Finish()
    {
        base.Finish();
    }
}
