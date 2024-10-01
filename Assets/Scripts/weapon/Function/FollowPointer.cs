using System;
using MainPlayer;
using UnityEngine;

public class FollowPointer : W_TInstance<FollowPointer>
{
    // public bool isAttack=false;
    // float GetAngle_Range360(Vector3 a,Vector3 b){
    //     if(Vector3.Cross(a,b).z<0){
    //         return Vector3.Angle(a,b);
    //     }
    //     else return -Vector3.Angle(a,b);
    // }
    // private Action lateupdate=>(Player.Instance.isAttack||isAttack)?()=>{transform.rotation=Quaternion.Euler(0, 0, GetAngle_Range360(transform.position-Player.Instance.gameObject.transform.position,Vector3.right));}:null;
    // private void LateUpdate() {
    //     lateupdate?.Invoke();
    // }
    /// <summary>
    /// 使用时将物体forward调整为右侧
    /// 获取鼠标位置使挂载脚本的物体始终朝向鼠标位置
    /// </summary>
    private Vector3 PointerPosOnScreen;//鼠标的屏幕位置
    private Vector3 PointerPos_worldPos;//鼠标的世界坐标
    private float AngleOfZ;//旋转后z的偏移量


    /// <summary>
    /// 360度Angle函数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    float GetAngle_Range360(Vector3 a,Vector3 b){
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
        AngleOfZ=GetAngle_Range360(PointerPos_worldPos-Player.Instance.transform.position,Vector3.right);//得到z偏移量
        transform.rotation =Quaternion.Euler(0, 0, AngleOfZ);
        if(Player.Instance.transform.GetChild(0).localScale.x<0){
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else{
            GetComponentInChildren<SpriteRenderer>().flipX=true;
        }
    }
    // private void Start()=>aa();
    // async UniTask aa(){
    //     while(true){
    //         await UniTask.Delay(1);
    //         UnityEngine.Debug.Log(AngleOfZ);
    //     }
    // }
    private Quaternion GetRotation(float Angle)=> Angle switch
    {
        >=-45 and <=45 => Quaternion.Euler(0, 0, 0),
        >= 45 and <= 135 => Quaternion.Euler(0, 0, 90),
        <=-45 and >=-135 => Quaternion.Euler(0, 0, -90),
        <=-135 and >=-180 => Quaternion.Euler(0, 0, 180),
        <=180 and >=135 => Quaternion.Euler(0, 0, 180),
        _ => throw new NotImplementedException(),
    };
    // transform.rotation= Quaternion.LookRotation((PointerPos_worldPos-transform.position).normalized);
    // transform.LookAt(PointerPos_worldPos);
    //以需调整transform.forward向右
    // Debug.DrawRay(transform.position, transform.forward * 100, Color.blue//绘制forward方向
}
