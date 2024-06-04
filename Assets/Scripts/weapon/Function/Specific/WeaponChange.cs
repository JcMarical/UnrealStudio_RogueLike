using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    /// <summary>
    /// 武器切换  传入为要加入背包的武器，索引为被替换的武器索引，0或1
    /// </summary>
    /// <param name="TargetWeapon"></param>
    /// <param name="TobeChangedWeapon_Index"></param>
    public static void ReplaceWeapon(GameObject TargetWeapon,int TobeChangedWeapon_Index){
        GameObject[] OwndWeapon=new GameObject[2];
        OwndWeapon[0]=StaticData.WeaponSlots[0].GetComponent<Weapon_slot>().Weapon_InSlot;
        if(TobeChangedWeapon_Index!=StaticData.CurrentWeapon_Index){
            Transform TempParent=OwndWeapon[TobeChangedWeapon_Index].transform.parent;
            OwndWeapon[TobeChangedWeapon_Index].SetActive(true);
            OwndWeapon[TobeChangedWeapon_Index].transform.parent=null;
            OwndWeapon[TobeChangedWeapon_Index]=TargetWeapon;
            TargetWeapon.SetActive(false);
            TargetWeapon.transform.parent=TempParent;
            TargetWeapon.transform.position=new Vector3(0,0,0);
        }//若为副武器，先激活,然后副武器丢掉（两武器互换父物体），换完新的武器重置位置
        else {
            Transform TempParent=OwndWeapon[TobeChangedWeapon_Index].transform.parent;
            OwndWeapon[TobeChangedWeapon_Index].transform.parent=null;
            OwndWeapon[TobeChangedWeapon_Index]=TargetWeapon;
            TargetWeapon.transform.parent=TempParent;
            TargetWeapon.transform.position=new Vector3(0,0,0);
        }
    }
    /// <summary>
    /// 副武器为空时填补空位
    /// </summary>
    /// <param name="TargetWeapon"></param>
    /// <param name="parent"></param>
    public static void FillWeaponBlank(GameObject TargetWeapon,Transform parent){
        StaticData.WeaponSlots[1].GetComponent<Weapon_slot>().Weapon_InSlot=TargetWeapon;
        TargetWeapon.transform.parent=parent;
        TargetWeapon.transform.position=new Vector3(0,0,0);
    }
    /// <summary>
    /// 更换主副武器，即更改当前武器索引
    /// </summary>
    public static void ChangeWeapon(){
        switch(StaticData.CurrentWeapon_Index){
            case 0:{
                Debug.Log("22");
                StaticData.WeaponSlots[0].GetComponent<Weapon_slot>().Weapon_InSlot.SetActive(false);
                StaticData.WeaponSlots[1].GetComponent<Weapon_slot>().Weapon_InSlot.SetActive(true);
                StaticData.WeaponSlots[1].GetComponent<Weapon_slot>().Weapon_InSlot.GetComponent<Weapon>().Attack();
                StaticData.CurrentWeapon_Index=1;
                break;
            }
            case 1:{
                Debug.Log("33");
                StaticData.WeaponSlots[1].GetComponent<Weapon_slot>().Weapon_InSlot.SetActive(false);
                StaticData.WeaponSlots[0].GetComponent<Weapon_slot>().Weapon_InSlot.SetActive(true);
                StaticData.WeaponSlots[0].GetComponent<Weapon_slot>().Weapon_InSlot.GetComponent<Weapon>().Attack();
                StaticData.CurrentWeapon_Index=0;
                break;
            }
        }
    }
}
