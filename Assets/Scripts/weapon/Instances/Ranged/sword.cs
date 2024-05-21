using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class sword : RangedWeapon{
    public GameObject TargetEnemy;//目标敌人
    public override void RangedWeaponAttack()
    {
        Debug.Log(StaticData.EnemiesWithin);
        if(StaticData.EnemiesWithin.Count!=0){
            TargetEnemy=StaticData.EnemiesWithin.ElementAt(0);//选取第一个敌人为目标
            Debug.Log("Attacked");
            //武器攻击动画
            //对TargetEnemy造成伤害
            //特殊效果
        }
       
    }
    private void Awake() {
        StaticData.OwndWeapon[StaticData.CurrentWeapon_Index]=gameObject;
    }
}
