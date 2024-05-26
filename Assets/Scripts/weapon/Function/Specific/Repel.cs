using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repel : MonoBehaviour
{
    /// <summary>
    /// 击退效果,击退方向（模为一），力度，被击退物体
    /// </summary>
    /// <param name="Direction"></param>
    /// <param name="Force_Value"></param>
    /// <param name="TargetEnemy"></param>
    public static void RepelEnemy(Vector3 Direction,float Force_Value,GameObject TargetEnemy){
        TargetEnemy.GetComponent<Rigidbody2D>().AddForce(Force_Value*Direction,ForceMode2D.Impulse);
    }
}
