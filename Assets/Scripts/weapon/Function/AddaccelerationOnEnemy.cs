using System.Data;
using UnityEditor.SceneManagement;
using UnityEngine;
/// <summary>
/// 用于实现给物体添加加速度
/// </summary>
class AddaccelerationOnEnemy:MonoBehaviour
{
    public int kind;//0为摩擦模拟，1为提供加速度
    public float acceleration;
    public Vector2 TargetVelocity;
    /// <summary>
    /// 用于初始化加速度
    /// </summary>
    /// <param name="acc"></param>
    public void Initialize1(float acc){
        kind=0;
        acceleration = acc;
    }
    public void Initialize2(float acc,Vector2 target){
        kind=1;
        TargetVelocity = target;
        acceleration=acc;
    }
    void FixedUpdate()
    {
        // transform.Translate(Velociy);
        // Velociy =  ( acceleration * Time.deltaTime*Velociy.normalized).magnitude>Velociy.magnitude?Vector2.zero:Velociy-acceleration * Time.deltaTime*Velociy.normalized;
        // if(Velociy.magnitude<ConstField.Instance.DeviationOfVelocity){
        //     transform.GetComponent<Enemy>().isRepelled=false;
        //     Destroy(this);
        // }
        
        //给刚体模拟摩擦力，速度减为零时清除本脚本
        if(kind==0)
            friction();
        if(kind==1)
            Addacceleration();
    }
    public void friction(){
        GetComponent<Rigidbody2D>().velocity-=GetComponent<Rigidbody2D>().velocity.normalized*acceleration*Time.fixedDeltaTime;
        if(GetComponent<Rigidbody2D>().velocity.magnitude < ConstField.Instance.DeviationOfVelocity){
            transform.GetComponent<Enemy>().isRepelled=false;
            Destroy(this);
        }
    }
    public void Addacceleration(){
        GetComponent<Rigidbody2D>().velocity+=GetComponent<Rigidbody2D>().velocity.normalized*acceleration*Time.fixedDeltaTime;
        if((GetComponent<Rigidbody2D>().velocity-TargetVelocity).magnitude < ConstField.Instance.DeviationOfVelocity){
            Destroy(this);
        }
    }
}