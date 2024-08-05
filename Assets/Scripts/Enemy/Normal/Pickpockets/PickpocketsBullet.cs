using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickpocketsBullet : MonoBehaviour
{
    public GameObject parentObject;
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
        //没有父对象就以自身为参数，没有就获取父对象
        GameObject parentObject = gameObject;
        if (transform.parent != null)
        {
            // 获取父对象
            parentObject = transform.parent.gameObject;
        }
        // 获取碰撞到的游戏对象
        GameObject target = collision.gameObject;

        // 判断目标是否具有 IDamageable 接口
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            // 获取父对象的 IncreasedInjury 和 Damage 属性
            float damageIncrease = parentObject.GetComponent<Enemy>().damageIncrease;
            float damage = parentObject.GetComponent<Enemy>().attackDamage[0];

            damageable.GetHit(damage * (1 + damageIncrease));
            //damageable.Repelled(force);
        }
    }
}
