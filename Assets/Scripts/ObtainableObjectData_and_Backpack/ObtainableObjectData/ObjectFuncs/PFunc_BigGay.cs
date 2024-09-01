using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_BigGay", menuName = "Data/ObtainableObjects/Func/BigGay", order = 20)]
public class PFunc_BigGay : PropFunc
{
    public GameObject BigGay;

    public override void OnAwake()
    {
        base.OnAwake();
        UseProp();
    }

    public override void UseProp()
    {
        base.UseProp();
        Instantiate(BigGay,Player.Instance.gameObject.transform.position,Quaternion.identity);
    }

    public override void Finish()
    {
        base.Finish();
    }
}
