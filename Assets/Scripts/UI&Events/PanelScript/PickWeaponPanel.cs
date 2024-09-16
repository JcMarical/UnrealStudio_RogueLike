using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickWeaponPanel : W_TInstance<PickWeaponPanel>
{
    public static int PickTarget=-1;
    private List<DragImage> weapons=new List<DragImage>();
    public DragImage dragingImage;
    public GameObject Test_PickWeapon;
    private void Awake(){
        weapons=new List<DragImage>();
        for(int i=0; i<3;i++){
            weapons.Add(transform.GetChild(i).GetComponent<DragImage>());
        }
    }
    private void OnEnable() {
        weapons[0].Weapon=Test_PickWeapon;
        weapons[1].Weapon=StaticData.Instance.GetActiveWeapon();
        weapons[2].Weapon=StaticData.Instance.GetInActiveWeapon();
        if(weapons[0]==null){
            weapons[0].PickWeapon_IsNull=false;
        }
    }
    public void CancelPick(){
        gameObject.SetActive(false);
    }
    public void ConfirmPick(){
        int t=-1;
        for(int i=1;i<3;i++){
            if(!weapons[i].IsInteractable){
                t=i;
                break;
            }
        }
        if(t!=-1)
            WeaponCtrl.Instance.PickWeapon(weapons[0].Weapon,t==1?1:0);
        gameObject.SetActive(false);
    }
    public void EndDrag(){
        DragImage target=GetDragImage();
        if(isChangable(target,dragingImage)){
            Exchange(target,dragingImage);
        }
        else{
            dragingImage?.ResetPos();
        }
        dragingImage=null;
    }
    void Exchange(DragImage a,DragImage b){
        int index=GetIndex(a,b);
        weapons[index].IsInteractable=weapons[index].IsInteractable^true;
        Transform temp=a.image.transform.parent;
        a.image.transform.parent=b.image.transform.parent;
        b.image.transform.parent=temp;
        a.ResetPos();
        b.ResetPos();
        if(!weapons[index].IsInteractable) {
            transform.GetChild(3).GetComponent<Button>().interactable = true;
        }
        else{
            transform.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }
    bool isChangable(DragImage a,DragImage b){
        if(a==null || b==null){
            return false;
        }
        if(a.index!=0&&b.index!=0){
            return false;
        }
        return a.IsInteractable&&b.IsInteractable;
    }
    int GetIndex(DragImage a,DragImage b){
        for(int i=0;i<3;i++){
            if(weapons[i]!=a&&weapons[i]!=b){
                Debug.Log(i);
                return i;
            }
        }
        return -1;
    }
    DragImage GetDragImage(){
        for(int i=0;i<3;i++){
            if(weapons[i]!=dragingImage&&isMouseIn(weapons[i].GetComponent<RectTransform>())){
                return weapons[i];
            }
        }
        return null;
    }
    bool isMouseIn(RectTransform target){
        // 获取鼠标在屏幕上的位置
        Vector2 mousePosition = Input.mousePosition;

        // 将屏幕坐标转换为 UI 元素的本地坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(target, mousePosition, null, out localPoint);

        // 判断鼠标是否在 UI 元素的 RectTransform 内部
        return target.rect.Contains(localPoint);

    }
}
