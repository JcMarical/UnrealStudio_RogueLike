using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Xml;
using UnityEngine;

public class LazyTurtleEnemy : Enemy
{
    public EnemyState killState;
    public Enemy enemy; // Enemy 实例引用
    public GameObject bulletPrefab; // 子弹的预制体
    public float bulletSpeed; // 子弹速度
    public bool killThroughout;
    public AttackEnemy attackEnemy;
    public float health;
    public int hit;
    // 调用这个方法来尝试攻击
    public void TryAttack()
    {
        Vector3 playerPosition = enemy.player.transform.position;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
        Vector2 bulletDirection = (playerPosition - gameObject.transform.position).normalized;
        attackEnemy = bulletInstance.GetComponent<AttackEnemy>();
        attackEnemy.enemy = enemy;
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
    }
    protected override void Awake()
    {
        base.Awake();

        chaseState = new LazyTurtleStateChase(this, enemyFSM, this);
        attackState = new LazyTurtleStateAttack(this, enemyFSM, this);
        deadState = new LazyTurtleStateDead(this, enemyFSM, this);
        killState=new LazyTurtleStateKill(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = chaseState;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        health = enemy.currentHealth;
        base.Start();
    }

    protected override void Update()
    {
        if (enemy.currentHealth <= 0)
        {
            enemyFSM.ChangeState(deadState);
        }
        base.Update();
        if (enemy.currentHealth!=health)
        {
            health = enemy.currentHealth;
            hit += 1;
        }
        if (hit>=7 && !killThroughout)
        {
            killThroughout = true;
            enemyFSM.ChangeState(killState);
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }



    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacles"))
        {
            // 计算反射方向
            Vector2 normal = collision.contacts[0].normal;
            moveDirection = Vector2.Reflect(moveDirection, normal);
        }
    }

    // 设置敌人的移动方向
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }
}
