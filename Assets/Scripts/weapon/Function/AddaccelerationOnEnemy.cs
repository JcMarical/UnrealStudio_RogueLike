using System.Data;
using UnityEditor.SceneManagement;
using UnityEngine;
/// <summary>
/// 用于实现给物体添加加速度
/// </summary>
class AddaccelerationOnEnemy:MonoBehaviour
{
    public float acceleration;
    /// <summary>
    /// 用于初始化加速度
    /// </summary>
    /// <param name="acc"></param>
    public void Initialize(float acc){
        acceleration = acc;
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
        GetComponent<Rigidbody2D>().velocity-=GetComponent<Rigidbody2D>().velocity.normalized*acceleration*Time.fixedDeltaTime;
        if(GetComponent<Rigidbody2D>().velocity.magnitude < ConstField.Instance.DeviationOfVelocity){
            transform.GetComponent<Enemy>().isRepelled=false;
            Destroy(this);
        }
    }
}