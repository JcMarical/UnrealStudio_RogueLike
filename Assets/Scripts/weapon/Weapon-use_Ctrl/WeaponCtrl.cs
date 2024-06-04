using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    /// <summary>
    /// 供玩家调用，进行攻击，触发一次
    /// </summary>
    public void Attack(){
        StaticData.WeaponSlots[StaticData.CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.GetComponent<Weapon>().Attack();
    }
    /// <summary>
    /// 切换主副武器
    /// </summary>
    public void ChangeWeapon(){
        Debug.Log("1");
        WeaponChange.ChangeWeapon();
    }
    /// <summary>
    /// 拾取武器，参数为拾取武器引用和被切换的武器索引（0为主武器，1为副武器）
    /// </summary>
    /// <param name="WeaponInScene"></param>
    /// <param name="ReplaceIndex"></param>
    public void PickWeapon(GameObject WeaponInScene,int ReplaceIndex){
        if(StaticData.WeaponSlots[1].GetComponent<Weapon_slot>().Weapon_InSlot!=null){
            WeaponChange.ReplaceWeapon(WeaponInScene,ReplaceIndex);
        }
        else{
            WeaponChange.FillWeaponBlank(WeaponInScene,transform.GetChild(1));
        }
    }
    /// <summary>
    /// 获取当前武器的攻击间隔
    /// </summary>
    /// <returns></returns>
    public float GetAttackInterval(){
        return StaticData.WeaponSlots[StaticData.CurrentWeapon_Index].GetComponent<Weapon_slot>().Weapon_InSlot.GetComponent<Weapon>().weaponData.AttachInterval;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            ChangeWeapon();
        }
    }
}
