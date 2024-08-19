using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 单例模式的实例引用，便于在其他类中访问
    public static CameraController instance;

    // 摄像机移动的速度
    public float speed;

    // 摄像机当前跟随的目标对象
    public Transform target;

    // 在脚本实例化时，设置单例引用
    private void Awake()
    {
        instance = this;
    }

    // 在每帧更新摄像机的位置，使其逐渐移动到目标对象的位置
    void Update()
    {
        if (target != null)
        {
            // 计算摄像机从当前位置移动到目标位置的插值，保持z轴不变
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                speed * Time.deltaTime
            );
        }
    }

    // 更改摄像机的跟随目标
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
