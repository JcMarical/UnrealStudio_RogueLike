using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Syrup", menuName = "Data/ObtainableObjects/Func/Syrup", order = 11)]
public class PFunc_Syrup : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        
    }

    public override void Finish()
    {
        base.Finish();
    }

    private IEnumerator _useProp()
    {
        Player.Instance.RealAttackSpeed += 1;
        yield return new WaitForSeconds(30f);
        Player.Instance.RealAttackSpeed -= 1;
        yield return null;
    }
}
