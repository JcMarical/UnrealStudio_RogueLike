using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    public static bool isChangable=true;
    /// <summary>
    /// 供玩家调用，进行攻击，触发一次并充能一次
    /// </summary>
    public void Attack(){
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Attack(()=>{
            StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Charge();
        });
        
    }
    /// <summary>
    /// 切换主副武器
    /// </summary>
    public void ChangeWeapon(){
        if(isChangable){
            isChangable=false;
            WeaponChange.ChangeWeapon(Attack);
        }
       
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
    /// 获取当前武器的详细信息
    /// </summary>
    /// <returns>当前武器详细信息，为WeaponData结构体链表，首元素为主武器，另为副武器</returns>
    public List<WeaponData> GetWeaponData(){
        List<WeaponData> weaponData = new List<WeaponData>
        {
            StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().weaponData,
            StaticData.Instance.GetInActiveWeapon().GetComponent<Weapon>().weaponData
        };
        return weaponData;
    }
    ///<summary>
    ///武器充能，参数为充能量
    ///</summary>
    public void Charge(int i){
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Charge(i);
    }
    ///<summary>
    ///武器充能,充能量为武器默认值
    ///</summary>
    public void Charge(){
        StaticData.Instance.GetActiveWeapon().GetComponent<Weapon>().Charge();
    }
    private void Update() {
        //if(Input.GetKeyDown(KeyCode.Space)){
        //    ChangeWeapon();
        //}
        transform.localPosition=Vector3.zero;
    }
}
