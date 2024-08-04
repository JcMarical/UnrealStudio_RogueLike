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
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
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
        GetComponent<Rigidbody2D>().velocity-=GetComponent<Rigidbody2D>().velocity.normalized*acceleration*Time.fixedDeltaTime;
        if(GetComponent<Rigidbody2D>().velocity.magnitude < ConstField.Instance.DeviationOfVelocity){
            transform.GetComponent<Enemy>().isRepelled=false;
            Destroy(this);
        }
    }
}