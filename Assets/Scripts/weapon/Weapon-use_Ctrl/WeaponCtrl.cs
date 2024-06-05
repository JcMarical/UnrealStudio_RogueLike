using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    /// <summary>
    /// 供玩家调用，进行攻击，触发一次
    /// </summary>
    public void Attack(){
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Attack();
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
            WeaponChange.PickWeapon(WeaponInScene,ReplaceIndex);
    }
    /// <summary>
    /// 获取当前武器的攻击间隔
    /// </summary>
    /// <returns></returns>
    public float GetAttackInterval(){
        return StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData.AttachInterval;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            ChangeWeapon();
        }
    }
}
