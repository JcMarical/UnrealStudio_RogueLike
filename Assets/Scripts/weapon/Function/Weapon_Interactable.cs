using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Interactable : MonoBehaviour,ITradable
{
    GameObject weaponCtrl;
    public bool IsBoughtable {
        get {return isBoughtable;}
        set {
            isBoughtable = value;
        }
    }
    public bool IsPickable{
        get { return isPickable;}
        set {
            //更改玩家交互目标检测
            isPickable = value;
        }
    }

    public int Price { get => GetComponent<Weapon>().weaponData.value; set => throw new System.NotImplementedException(); }

    GoodType ITradable.GoodType { get => GoodType.Weapon; set => throw new System.NotImplementedException(); }

    private void Awake() {
        weaponCtrl=WeaponCtrl.Instance.gameObject;
        isPickable=transform.parent.parent==weaponCtrl?false:true;
    }
    public void BeBought(Vector3 startPos)
    {
        isBoughtable=false;
        isPickable=true;
        WeaponCtrl.Instance.ShowPickWeaponPanel(gameObject);
    }

    public void BeSoldOut()
    {
        throw new System.NotImplementedException();
    }
    bool isBoughtable;
    bool isPickable;
}
