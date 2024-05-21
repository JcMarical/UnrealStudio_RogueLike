using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repel : MonoBehaviour
{
    //击退效果
    public static void RepelEnemy(Vector3 Direction,float Force_Value,GameObject TargetEnemy){
        TargetEnemy.GetComponent<Rigidbody2D>().AddForce(Force_Value*Direction,ForceMode2D.Impulse);
    }
}
