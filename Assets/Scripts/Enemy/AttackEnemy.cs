using Cysharp.Threading.Tasks;
using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 贴身攻击的敌人，判断不能有友伤害
/// </summary>
public class AttackEnemy : MonoBehaviour
{
    public Enemy enemy;

    private float time;
    public int damageIndex;
    public bool canDamageEnemy; //是否可以伤害敌人
    public bool canDamageSelf;  //是否可以伤害自己

    private void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;
    }


    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (time <= 0f)
        {
            // 获取碰撞到的游戏对象
            GameObject target = collision.gameObject;

            // 判断目标是否具有 IDamageable 接口
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (!ReferenceEquals(damageable,null) && (target.CompareTag("Player") || canDamageEnemy) && (target.gameObject != enemy.gameObject || canDamageSelf))
            {
                // 获取父对象的 damageIncrease 和 Damage 属性
                float damageIncrease = enemy.damageIncrease;
                float damage = enemy.attackDamage[damageIndex];

                // 获取父对象的 type 属性
                //string type = parentObject.GetComponent<Enemy>().enemyType.ToString();
                Repel(target);

                damageable.GetHit(Mathf.Floor(damage * (1 + damageIncrease)));
                time = 0.2f;
            }

        }
    }

    /// 玩家击退测试代码
    void Repel(GameObject target)
    {
        if (!Player.Instance.isRepel && (!Player.Instance.isInvincible && !Player.Instance.areInvincle))
        {
            Player.Instance.playerAnimation.TransitionType(PlayerAnimation.playerStates.Harm);
            Player.Instance.isRepel = true;
            Player.Instance.repelDirection =(new Vector3(target.transform.position.x,target.transform.position.y,0) - new Vector3(transform.position.x,transform.position.y,0)).normalized;
            Player.Instance.attackEnemy = gameObject;
            Player.Instance.Force = enemy.force;
            if (gameObject.layer == 10)//子弹层级对应的索引
            {
                Destroy(gameObject);
            }
        }
    }
}
