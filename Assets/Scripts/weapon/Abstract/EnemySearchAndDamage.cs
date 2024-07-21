using System.Collections;
using System.Collections.Generic;
using MainPlayer;
using UnityEngine;

public class EnemySearchAndDamage : MonoBehaviour
{
    /// <summary>
    /// 索敌脚本，挂载到Range_Collider
    /// </summary>
    /*
    进入检查tag入链表
    离开检查链表包含，Remove
    */
    public enum AttackKind{
        ballisticAndNonPenetrating,//弹道非穿透
        ballisticAndPenetrating,//弹道穿透
        melee,//近战
        onlyDamage//仅伤害
    }    
    private AttackKind AttacKind;  
    private void Start() {
        Bullet temp;
        Weapon temp1;
        TryGetComponent<Bullet>(out temp);
        if(temp!=null) {
            switch(temp.BulletKind){
                case 0:
                AttacKind=AttackKind.ballisticAndPenetrating;
                break;
                case 1:
                AttacKind=AttackKind.ballisticAndNonPenetrating;
                break;
            }
        }
        else{
            TryGetComponent<Weapon>(out temp1);
            if(temp1!=null) {
                AttacKind=AttackKind.melee;
            }
        }
    }  
    protected void OnTriggerEnter2D(Collider2D other) {
        if(other!=null) {
            Debug.Log(other);
            if(other.CompareTag(ConstField.Instance.EnemyTag)){
                Debug.Log("11");
                Repel(other.gameObject);
                other.GetComponent<WeaponTest_Enemy>().GetHit();//待替换为实际敌人受伤方法
            }
        }
   }
    private void Repel(GameObject targetEnemy){
        if(AttacKind==AttackKind.onlyDamage) return;
        Vector3 Direction=(targetEnemy.transform.position - Player.Instance.transform.position).normalized;
        switch(AttacKind){
            #region 弹道非穿透击退实现
            case AttackKind.ballisticAndNonPenetrating:
                Vector3 velocity_Temp=targetEnemy.GetComponent<Rigidbody2D>().velocity;
                if(targetEnemy.GetComponent<Rigidbody2D>().velocity.magnitude>ConstField.Instance.DeviationOfVelocity){
                    targetEnemy.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
                }
                targetEnemy.GetComponent<Rigidbody2D>().velocity=
                WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*Direction+((velocity_Temp.magnitude<0.5f)?Vector2.zero:velocity_Temp);
                targetEnemy.AddComponent<AddaccelerationOnEnemy>();
                targetEnemy.GetComponent<AddaccelerationOnEnemy>().acceleration=2*velocity_Temp;
                targetEnemy.GetComponent<AddaccelerationOnEnemy>().targetVelociy=velocity_Temp;
                break;
            #endregion

            #region 弹道穿透击退实现
            case AttackKind.ballisticAndPenetrating:
                break;
            #endregion

            #region 近战击退实现
            case AttackKind.melee:
                

                break;
            #endregion

                
        }
   }
}
