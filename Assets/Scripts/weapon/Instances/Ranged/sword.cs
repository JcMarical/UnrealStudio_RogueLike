using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class sword : RangedWeapon{
    public GameObject TargetEnemy;//目标敌人
    public override void RangedWeaponAttack(Action action)
    {
        Debug.Log(StaticData.Instance.EnemiesWithin);
        if(StaticData.Instance.EnemiesWithin.Count!=0){
            TargetEnemy=StaticData.Instance.EnemiesWithin.ElementAt(0);//选取第一个敌人为目标
            Debug.Log("Attacked");
            //武器攻击动画
            //对TargetEnemy造成伤害
            //攻击成功执行action
            //特殊效果
        }
    }
}
