using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrentWeaponChange : MonoBehaviour
{
    //切换到指定武器
    public static void changeToIndex(int TargetIndex){
        changeToGameObject(StaticData.ChangeableWeapon.ElementAt(TargetIndex));
    }
    //切换到下一个武器
    public static void changeToNext(){
        //检测是否为最后一个武器
        if(StaticData.ChangeableWeapon.IndexOf(StaticData.CurrentWeapon)!=StaticData.ChangeableWeapon.Count-1){
            changeToGameObject(StaticData.ChangeableWeapon.ElementAt(StaticData.ChangeableWeapon.IndexOf(StaticData.CurrentWeapon)+1));
        }
        else
        {
             changeToGameObject(StaticData.ChangeableWeapon.ElementAt(0));
        }
    }
    //切换到上一个武器
    public static void changeToLast(){
        //检测是否为第一个武器
        if(StaticData.ChangeableWeapon.IndexOf(StaticData.CurrentWeapon)!=0){
            changeToGameObject(StaticData.ChangeableWeapon.ElementAt(StaticData.ChangeableWeapon.IndexOf(StaticData.CurrentWeapon)-1));
        }
        else
        {
             changeToGameObject(StaticData.ChangeableWeapon.ElementAt(StaticData.ChangeableWeapon.Count-1));
        }
    }
    //武器切换
    private static void changeToGameObject(GameObject TargetWeapon){
        Destroy(StaticData.CurrentWeapon);
        StaticData.CurrentWeapon=Instantiate(TargetWeapon,null);
    }
}
