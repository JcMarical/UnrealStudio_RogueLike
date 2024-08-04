using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickpocketss : MonoBehaviour
{
    public Enemy enemy; // Enemy 实例引用
    public GameObject bulletPrefab; // 子弹的预制体
    public float bulletSpeed; // 子弹速度
    public void Start()
    {
        enemy=FindObjectOfType<Enemy>();
    }
    // 调用这个方法来尝试攻击
    public void TryAttack()
    {
        if (enemy.IsPlayerInVisualRange())
        {
            Debug.Log("True");
            Vector3 playerPosition = enemy.player.transform.position;
            GameObject bulletInstance = Instantiate(bulletPrefab, enemy.transform.position, Quaternion.identity);
            Vector2 bulletDirection = (playerPosition - gameObject.transform.position).normalized;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
        }
    }
}
