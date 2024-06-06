using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanimctrl : WeaponAnimCtrl
{
    public override void OnAttack()
    {
        Continue=true;
    }
    public override void AnimBegin()
    {
        Continue=false;
    }
}
