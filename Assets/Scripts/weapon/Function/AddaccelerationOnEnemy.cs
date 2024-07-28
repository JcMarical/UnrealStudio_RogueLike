using UnityEditor.SceneManagement;
using UnityEngine;
/// <summary>
/// 用于实现给物体添加加速度
/// </summary>
class AddaccelerationOnEnemy:MonoBehaviour
{
    public Vector2 acceleration;
    public Vector2 targetVelociy;
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(acceleration*GetComponent<Rigidbody2D>().mass,ForceMode2D.Force);
        if((GetComponent<Rigidbody2D>().velocity-targetVelociy).magnitude < ConstField.Instance.DeviationOfVelocity){
            Destroy(this);
        }
    }
}