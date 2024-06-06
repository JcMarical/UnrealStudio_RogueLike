using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeTest : MeleeWeapon
{
    
    public GameObject TargetEnemy;//目标敌人
    public override void RangedWeaponAttack(Action action)
    {
        //调用敌人受伤方法
    }
    public void AttackeEnd(){

    }

    public override void SetActiveCollider()
    {
        
    }
}