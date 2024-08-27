using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Loupe", menuName = "Data/ObtainableObjects/Func/Loupe", order = 10)]
public class PFunc_Loupe : PropFunc
{
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.StartCoroutine(func());
    }

    public override void Finish()
    {
        base.Finish();
    }

    private IEnumerator func()
    {
        //TODO:增大攻击范围
        yield return new WaitForSeconds(30);
        //TODO:还原
        yield return null;
    }
}
