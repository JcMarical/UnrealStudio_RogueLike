using UnityEngine;
using UnityEngine.InputSystem;
public class Weapon_Interable : Interactable
{
    public override void OnInteractBegin(InputAction.CallbackContext context){
        //唤起ui，选择是否更换武器
        OnInteract();
    }
    public override void OnInteract(){
        //禁用移动
    }
    public override void OnInteractEnd(){
        //关闭UI，恢复移动
    }
}
