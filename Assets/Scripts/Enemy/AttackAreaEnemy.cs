using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaEnemy : MonoBehaviour
{
    public Enemy enemy;

    private float time;
    public int damageIndex;

    private void Awake()
    {
        if (TryGetComponent<Enemy>(out Enemy enemy))
            this.enemy = enemy;
    }

    private void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (time <= 0f)
        {
            // 获取碰撞到的游戏对象
            GameObject target = collision.gameObject;

            // 判断目标是否具有 IDamageable 接口
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // 获取父对象的 damageIncrease 和 Damage 属性
                float damageIncrease = enemy.damageIncrease;
                float damage = enemy.attackDamage[damageIndex];

                // 获取父对象的 type 属性
                //string type = parentObject.GetComponent<Enemy>().enemyType.ToString();

                damageable.GetHit(damage * (1 + damageIncrease));
                //damageable.Repelled(force, type);

                Destroy(gameObject);
            }
            time = 0.2f;
        }
    }
}
