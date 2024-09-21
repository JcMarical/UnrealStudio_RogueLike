using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragImage : MonoBehaviour
{
    public int index;
    private GameObject weapon;
    public bool PickWeapon_IsNull=true;
    public GameObject Weapon{
        get { return weapon; }
        set { 
            weapon = value; 
            if(value != null){
                IsInteractable=true;
                image.gameObject.SetActive(true);
                image.sprite=weapon.GetComponent<Weapon>().weaponData.sprite;
            }
            else if(value==null&&index!=0){
                isInteractable=true;
                image.gameObject.SetActive(true);
            }
            else{
                isInteractable=false;
                image.gameObject.SetActive(false);
            }
        }
    }
    bool isInteractable=true;
    public bool IsInteractable{
        get{return isInteractable;}
        set{
            if(isInteractable&&!value){
                image.color=new Color(image.color.r, image.color.g,image.color.b,0.5f);
            }
            else if(!isInteractable&&value){
                image.color=new Color(image.color.r, image.color.g,image.color.b,1);
            }
            isInteractable=value;
        }
    }
    public Image image=>transform.GetChild(0).GetChild(0).GetComponent<Image>();
    public Vector3 prePos=>image.transform.parent.position;
    Action update;
    bool isDrag;
    void Update()
    {
        update?.Invoke();
        if(Input.GetMouseButtonDown(0)&&isMouseIn()&&isInteractable&&PickWeapon_IsNull){
            PickWeaponPanel.Instance.dragingImage=this;
            isDrag=true;
            update+=()=>{
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(  
                    image.transform.parent.GetComponent<RectTransform>(),  
                    Input.mousePosition,  
                    Camera.main,  
                    out Vector2 mousePosInCanvas))   
                image.rectTransform.anchoredPosition = mousePosInCanvas;     
            };
        }
        else if(Input.GetMouseButtonUp(0)&&PickWeapon_IsNull){
            update=null;
            if(isDrag){
                PickWeaponPanel.Instance?.EndDrag();
            }
        ResetPos();
        }
}
    
    bool isMouseIn(){
        // 获取鼠标在屏幕上的位置
        Vector2 mousePosition = Input.mousePosition;

        // 将屏幕坐标转换为 UI 元素的本地坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), mousePosition, Camera.main, out localPoint);

        // 判断鼠标是否在 UI 元素的 RectTransform 内部
        return GetComponent<RectTransform>().rect.Contains(localPoint);
    }
    public void ResetPos(){
        image.transform.position=prePos;
    }
}