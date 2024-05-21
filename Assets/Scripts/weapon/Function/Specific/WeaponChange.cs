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
        if(TobeChangedWeapon_Index!=StaticData.CurrentWeapon_Index){
            Transform TempParent=StaticData.OwndWeapon[TobeChangedWeapon_Index].transform.parent;
            StaticData.OwndWeapon[TobeChangedWeapon_Index].SetActive(true);
            StaticData.OwndWeapon[TobeChangedWeapon_Index].transform.parent=null;
            StaticData.OwndWeapon[TobeChangedWeapon_Index]=TargetWeapon;
            TargetWeapon.SetActive(false);
            TargetWeapon.transform.parent=TempParent;
        }//若为副武器，先激活,然后副武器丢掉（两武器互换父物体），换完新的武器重置位置
        else {
            Transform TempParent=StaticData.OwndWeapon[TobeChangedWeapon_Index].transform.parent;
            StaticData.OwndWeapon[TobeChangedWeapon_Index].transform.parent=null;
            StaticData.OwndWeapon[TobeChangedWeapon_Index]=TargetWeapon;
            TargetWeapon.transform.parent=TempParent;
        }
    }
    /// <summary>
    /// 更换主副武器，即更改当前武器索引
    /// </summary>
    public static void ChangeWeapon(){
        switch(StaticData.CurrentWeapon_Index){
            case 0:
            StaticData.OwndWeapon[0].SetActive(false);
            StaticData.OwndWeapon[1].SetActive(true);
            StaticData.CurrentWeapon_Index=1;
            break;
            case 1:
            StaticData.OwndWeapon[1].SetActive(false);
            StaticData.OwndWeapon[0].SetActive(true);
            StaticData.CurrentWeapon_Index=0;
            break;
        }
    }
}
