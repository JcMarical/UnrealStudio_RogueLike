using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FollowPointer : MonoBehaviour
{
    /// <summary>
    /// 使用时将物体forward调整为右侧
    /// 获取鼠标位置使挂载脚本的物体始终朝向鼠标位置
    /// </summary>
    private Vector3 PointerPosOnScreen;//鼠标的屏幕位置
    private Vector3 PointerPos_worldPos;//鼠标的世界坐标
    private float AngleOfZ;//旋转后z的偏移量


    
    float GetAngle_Range360(Vector3 a,Vector3 b){
        //解决自带angle函数不能得到360度范围问题
        if(Vector3.Cross(a,b).z<0){
            return Vector3.Angle(a,b);
        }
        else return -Vector3.Angle(a,b);
    }
    void Update()
    {
        PointerPosOnScreen=Input.mousePosition;
        PointerPosOnScreen.z=10;//camera自带-10的深度，z改为10防止转换后z不等于0
        PointerPos_worldPos=Camera.main.ScreenToWorldPoint(PointerPosOnScreen);//屏幕坐标转为世界坐标
        AngleOfZ=GetAngle_Range360(PointerPos_worldPos,Vector3.right);//得到z偏移量
        transform.rotation=Quaternion.Euler(0,0,AngleOfZ);
        // transform.rotation= Quaternion.LookRotation((PointerPos_worldPos-transform.position).normalized);
        // transform.LookAt(PointerPos_worldPos);
        //以需调整transform.forward向右
        // Debug.DrawRay(transform.position, transform.forward * 100, Color.blue//绘制forward方向
        
    }
}
