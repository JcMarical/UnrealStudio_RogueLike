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
    public AttackKind AttacKind;
    CinemachineBasicMultiChannelPerlin noiseProfile;
    private void Start() {
        /*
        判断挂载对象，分子弹和近战武器
        并判断击退方式
        */
        noiseProfile=Camera.main.GetComponent<CinemachineBrain>()?.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }  
    protected void OnTriggerEnter2D(Collider2D other) {
        if(other!=null) {
            
            if(other.CompareTag(ConstField.Instance.EnemyTag)){Debug.Log("asdihadifbodfbgoshgbnjpfjno");
                //击退
                if(!other.GetComponent<Enemy>().isRepelled){
                    Repel(other.gameObject);
                }
                //伤害结算
                (float Damage,bool ShowDamage)=getWeaponDirectHitValue(other.GetComponent<Enemy>());
                other.GetComponent<Enemy>().GetHit(Mathf.CeilToInt(Damage));//实际敌人受伤方法
                //事件处理
                WeaponCtrl.Instance.OnDamage?.Invoke(other.gameObject);//道具特殊效果

                if(ShowDamage){
                    //伤害跳字
                    CameraShake();
                }
                WeaponCtrl.Instance.SettleSpecialEffect(other.gameObject);
                if(AttacKind==AttackKind.ballisticAndNonPenetrating)
                    DesSelf();
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
                targetEnemy.GetComponent<Enemy>().isRepelled=true;
                targetEnemy.GetComponent<Rigidbody2D>().AddForce(WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*GetComponent<Rigidbody2D>().velocity.normalized+targetEnemy.GetComponent<Enemy>().CurrentSpeed*targetEnemy.GetComponent<Enemy>().moveDirection,ForceMode2D.Impulse);
                targetEnemy.AddComponent<AddaccelerationOnEnemy>().Initialize1(isStill?targetEnemy.GetComponent<Enemy>().acceleration:2*velocity_Temp);
                break;
            #endregion

            #region 弹道穿透击退实现
            case AttackKind.ballisticAndPenetrating:
                Vector2 temp=targetEnemy.GetComponent<Rigidbody2D>().velocity;
                targetEnemy.GetComponent<Rigidbody2D>().velocity=temp/2;
                targetEnemy.AddComponent<AddaccelerationOnEnemy>().Initialize2(isStill?targetEnemy.GetComponent<Enemy>().acceleration:2*velocity_Temp,temp);
                break;
            #endregion

            #region 近战击退实现
            case AttackKind.melee:
                targetEnemy.GetComponent<Enemy>().isRepelled=true;
                targetEnemy.GetComponent<Rigidbody2D>().AddForce(WeaponCtrl.Instance.GetWeaponData()[0].ExpulsionStrength*ConstField.Instance.LengthPerCeil*Direction+targetEnemy.GetComponent<Enemy>().CurrentSpeed*targetEnemy.GetComponent<Enemy>().moveDirection,ForceMode2D.Impulse);
                targetEnemy.AddComponent<AddaccelerationOnEnemy>().Initialize1(isStill?targetEnemy.GetComponent<Enemy>().acceleration:2*velocity_Temp);
                break;
            #endregion  
        }
   }
    /// <summary>
    /// 攻击伤害计算
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>伤害和倍率是否大于2</returns>
    public (float, bool) getWeaponDirectHitValue(Enemy enemy)
    {
        float Mutiple = (1 + PlayerBuffMonitor.Instance.InjuryBuff) * (1 / (Mathf.Log(enemy.rb.mass, 2) + 1)) * enemy.getDamageMultiple;

        return ((Player.Instance.playerData.playerAttack + UnityEngine.Random.Range(-0.2f, 0.2f)) * WeaponCtrl.Instance.GetWeaponData()[0].DamageValue_bas * Mutiple,
        Mutiple >= 2 ? true : false);
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
    private void DesSelf(){
        //销毁特效
        Destroy(gameObject);
    }
}
