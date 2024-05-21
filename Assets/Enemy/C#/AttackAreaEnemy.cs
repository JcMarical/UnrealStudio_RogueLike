using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ȡ������
        GameObject parentObject = transform.parent.gameObject;

        // ��ȡ��ײ������Ϸ����
        GameObject target = collision.gameObject;

        // �ж�Ŀ���Ƿ���� IDamageable �ӿ�
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // ��ȡ������� IncreasedInjury �� Damage ����
            float increasedInjury = parentObject.GetComponent<Enemy>().IncreasedInjury[0];
            float damage = parentObject.GetComponent<Enemy>().attackDamage[0];

            // ��ȡ������� force �� type ����
            float force = parentObject.GetComponent<Enemy>().force[0];
            string type = parentObject.GetComponent<Enemy>().enemyType.ToString();

            damageable.GetHit(damage, increasedInjury);
            damageable.Repelled(force, type);
        }
    }
}
