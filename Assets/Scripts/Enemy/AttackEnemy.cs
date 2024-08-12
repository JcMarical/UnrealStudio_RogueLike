using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������ĵ��ˣ��жϲ��������˺�
/// </summary>
public class AttackEnemy : MonoBehaviour
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
            // ��ȡ��ײ������Ϸ����
            GameObject target = collision.gameObject;

            // �ж�Ŀ���Ƿ���� IDamageable �ӿ�
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable != null && target.CompareTag("Player"))
            {
                // ��ȡ������� damageIncrease �� Damage ����
                float damageIncrease = enemy.damageIncrease;
                float damage = enemy.attackDamage[damageIndex];

                // ��ȡ������� type ����
                //string type = parentObject.GetComponent<Enemy>().enemyType.ToString();

                damageable.GetHit(damage * (1 + damageIncrease));
                //damageable.Repelled(force, type);
            }
            time = 0.2f;
        }
    }
}
