using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_ChocolateCookie", menuName = "Data/ObtainableObjects/Func/ChocolateCookie", order = 9)]
public class PFunc_ChocolateCookie : PropFunc
{

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.StartCoroutine(_usePorp());
    }

    public override void Finish()
    {
        base.Finish();
    }

    private IEnumerator _usePorp()
    {
        Player.Instance.realPlayerAttack += 1;
        yield return new WaitForSeconds(30f);
        Player.Instance.realPlayerAttack -= 1;
        isDone = true;
        Finish();
        yield return null;
    }
}
