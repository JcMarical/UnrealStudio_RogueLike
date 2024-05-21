using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    /// <summary>
    /// 索敌脚本，挂载到Range_Collider
    /// </summary>
    public string EnemyTag="Enemy";//敌人tag
    /*
    进入检查tag入链表
    离开检查链表包含，Remove
    */
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.CompareTag(EnemyTag)){
            StaticData.EnemiesWithin.Add(other.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(StaticData.EnemiesWithin.Contains(other.gameObject)){
           StaticData.EnemiesWithin.Remove(other.gameObject);
        }
    }
}
