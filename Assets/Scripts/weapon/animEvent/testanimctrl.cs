using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class testanimctrl : WeaponAnimCtrl
{
    public override void OnAttack()
    {
        GetComponent<Animator>().SetTrigger("Continue");
    }
}
