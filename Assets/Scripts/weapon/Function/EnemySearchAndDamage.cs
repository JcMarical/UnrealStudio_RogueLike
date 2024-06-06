using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchAndDamage : MonoBehaviour
{
    /// <summary>
    /// 索敌脚本，挂载到Range_Collider
    /// </summary>
    public string EnemyTag="Enemy";//敌人tag
    /*
    进入检查tag入链表
    离开检查链表包含，Remove
    */        
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.CompareTag(EnemyTag)){
            Debug.Log("11");
            other.GetComponent<WeaponTest_Enemy>().GetHit();
        }
   }
}
