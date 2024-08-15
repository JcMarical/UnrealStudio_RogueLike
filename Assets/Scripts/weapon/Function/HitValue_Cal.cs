using System.Collections;
using System.Collections.Generic;
using MainPlayer;
using UnityEngine;

public class HitValue_Cal
{
    public static (float,bool) getWeaponDirectHitValue(Enemy enemy){
        float Mutiple=(1+PlayerBuffMonitor.Instance.InjuryBuff)* (1 / (Mathf.Log(2, enemy.rb.mass) + 1)) * enemy.getDamageMultiple;
        return ((Player.Instance.playerData.playerAttack+Random.Range(-0.2f,0.2f))*WeaponCtrl.Instance.GetWeaponData()[0].DamageValue_bas*Mutiple,
        Mutiple>=2?true:false);
    }
}
