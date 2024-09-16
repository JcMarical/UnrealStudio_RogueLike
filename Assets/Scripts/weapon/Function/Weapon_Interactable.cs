using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Interactable : MonoBehaviour,ITradable
{
    GameObject weaponCtrl;
    bool isInteractable;
    public bool IsInteractable{
        get { return isInteractable;}
        set {
            //更改玩家交互目标检测
            isInteractable = value;
        }
    }

    public int Price { get => GetComponent<Weapon>().weaponData.value; set => throw new System.NotImplementedException(); }

    GoodType ITradable.GoodType { get => GoodType.Weapon; set => throw new System.NotImplementedException(); }

    private void Awake() {
        weaponCtrl=WeaponCtrl.Instance.gameObject;
        isInteractable=transform.parent.parent==weaponCtrl?false:true;
    }
    public void BeBought(Vector3 startPos)
    {
        
    }

    public void BeSoldOut()
    {
        throw new System.NotImplementedException();
    }
}
