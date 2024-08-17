using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MainPlayer;
using Unity.Mathematics;
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
    CinemachineBasicMultiChannelPerlin noiseProfile;
    private void Start() {
        /*
        判断挂载对象，分子弹和近战武器
        并判断击退方式
        */
        noiseProfile=Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Bullet temp=GetComponent<Bullet>();
        Weapon temp1=transform.parent.GetComponent<Weapon>();
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
            if(temp1!=null) {
                AttacKind=AttackKind.melee;
            }
        }
    }  
    protected void OnTriggerEnter2D(Collider2D other) {
        if(other!=null) {
            if(other.CompareTag(ConstField.Instance.EnemyTag)){
                //击退
                if(!other.GetComponent<Enemy>().isRepelled){
                    Repel(other.gameObject);
                }

                //伤害结算
                (float Damage,bool ShowDamage)=getWeaponDirectHitValue(other.GetComponent<Enemy>());
                other.GetComponent<Enemy>().GetHit(Mathf.CeilToInt(Damage));//实际敌人受伤方法
                CameraShake();
                if(ShowDamage){
                    //伤害跳字
                    CameraShake();
                }
            }
        }
   }
   /// <summary>
   /// 击退目标敌人
   /// </summary>
   /// <param name="targetEnemy"></param>
    private void Repel(GameObject targetEnemy){
        if(AttacKind==AttackKind.onlyDamage) return;
        Vector2 Direction=(targetEnemy.transform.position - Player.Instance.transform.position).normalized;
        float velocity_Temp=targetEnemy.GetComponent<Enemy>().chaseSpeed;
        bool isStill=targetEnemy.GetComponent<Rigidbody2D>().velocity.magnitude<ConstField.Instance.DeviationOfVelocity;
        switch(AttacKind){
            #region 弹道非穿透击退实现
            case AttackKind.ballisticAndNonPenetrating:
                // targetEnemy.GetComponent<Enemy>().isRepelled=true;
                // if(!isStill){
                //     targetEnemy.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
                // }
                // // targetEnemy.GetComponent<Rigidbody2D>().velocity=
                // // WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*Direction+((velocity_Temp.magnitude<0.5f)?Vector2.zero:velocity_Temp);
                // targetEnemy.AddComponent<AddaccelerationOnEnemy>();
                // targetEnemy.GetComponent<AddaccelerationOnEnemy>().Velociy=WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*Direction;
                // targetEnemy.GetComponent<AddaccelerationOnEnemy>().acceleration=isStill?targetEnemy.GetComponent<Enemy>().acceleration:2*velocity_Temp;
                // //targetEnemy.GetComponent<AddaccelerationOnEnemy>().targetVelociy=velocity_Temp;
                // break;
            #endregion

            #region 弹道穿透击退实现
            case AttackKind.ballisticAndPenetrating:
                break;
            #endregion

            #region 近战击退实现
            case AttackKind.melee:
                targetEnemy.GetComponent<Enemy>().isRepelled=true;
                targetEnemy.GetComponent<Rigidbody2D>().AddForce(WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*Direction+targetEnemy.GetComponent<Enemy>().CurrentSpeed*targetEnemy.GetComponent<Enemy>().moveDirection,ForceMode2D.Impulse);
                // targetEnemy.GetComponent<Rigidbody2D>().velocity=
                // WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*Direction+((velocity_Temp.magnitude<0.5f)?Vector2.zero:velocity_Temp);
                targetEnemy.AddComponent<AddaccelerationOnEnemy>().Initialize(isStill?targetEnemy.GetComponent<Enemy>().acceleration:2*velocity_Temp);
                //targetEnemy.GetComponent<AddaccelerationOnEnemy>().targetVelociy=velocity_Temp;
                break;
            #endregion  
        }
   }
    /// <summary>
    /// 攻击伤害计算
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>伤害和倍率是否大于2</returns>
   public (float,bool) getWeaponDirectHitValue(Enemy enemy){
        float Mutiple=(1+PlayerBuffMonitor.Instance.InjuryBuff)* (1 / (Mathf.Log(2, enemy.rb.mass) + 1)) * enemy.getHitMultiple;
        return ((Player.Instance.playerData.playerAttack+UnityEngine.Random.Range(-0.2f,0.2f))*WeaponCtrl.Instance.GetWeaponData()[0].DamageValue_bas*Mutiple,
        Mutiple>=2?true:false);
    }
    /// <summary>
    /// 相机抖动
    /// </summary>
    /// <param name="duration">时间</param>
    /// <param name="amplitude">幅度</param>
    /// <param name="frequency">频率</param>
    public void CameraShake(float duration = 0.25f, float amplitude = 1f, float frequency = 1f)
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = amplitude;
            noiseProfile.m_FrequencyGain = frequency;
            Invoke(nameof(StopShaking), duration);
        }
    }
    private void StopShaking()
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = 0f;
            noiseProfile.m_FrequencyGain = 0f;
        }
    }
}
