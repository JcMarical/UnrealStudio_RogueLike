using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;

[CreateAssetMenu(fileName = "PFunc_RedApple", menuName = "Data/ObtainableObjects/Func/RedApple", order = 7)]
public class PFunc_RedApple : PropFunc
{
    public float healthEffectValue = 1;
    public float attackEffectValue = 0.2f;
    public override void OnAwake()
    {
        base.OnAwake();
        Player.Instance.realMaxHealth += healthEffectValue;
        Player.Instance.playerData.playerAttack += attackEffectValue;
    }

    public override void UseProp()
    {
        base.UseProp();        
    }

    public override void Finish()
    {
        base.Finish();
    }
}
