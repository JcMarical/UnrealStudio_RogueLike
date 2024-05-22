using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    /// <summary>
    /// 供玩家调用，进行攻击，触发一次
    /// </summary>
    public void Attack(){
        StaticData.OwndWeapon[StaticData.CurrentWeapon_Index].GetComponent<Weapon>().Attack();
    }
    /// <summary>
    /// 切换主副武器
    /// </summary>
    public void ChangeWeapon(){
        WeaponChange.ChangeWeapon();
    }
    /// <summary>
    /// 拾取武器，参数为拾取武器引用和被切换的武器索引（0为主武器，1为副武器）
    /// </summary>
    /// <param name="WeaponInScene"></param>
    /// <param name="ReplaceIndex"></param>
    public void PickWeapon(GameObject WeaponInScene,int ReplaceIndex){
        if(StaticData.OwndWeapon[1]!=null){
            WeaponChange.ReplaceWeapon(WeaponInScene,ReplaceIndex);
        }
        else{
            WeaponChange.FillWeaponBlnnk(WeaponInScene,transform.GetChild(1));
        }
    }
    /// <summary>
    /// 获取当前武器的攻击间隔
    /// </summary>
    /// <returns></returns>
    public float GetAttackInterval(){
        return StaticData.OwndWeapon[StaticData.CurrentWeapon_Index].GetComponent<Weapon>().weaponData.AttachInterval;
    }
    private void Awake() {
        StaticData.OwndWeapon[0]=transform.GetChild(0).GetChild(0).gameObject!=null?transform.GetChild(0).GetChild(0).gameObject:null;
        StaticData.OwndWeapon[1]=transform.GetChild(1).GetChild(0).gameObject!=null?transform.GetChild(1).GetChild(0).gameObject:null;
    }
}
