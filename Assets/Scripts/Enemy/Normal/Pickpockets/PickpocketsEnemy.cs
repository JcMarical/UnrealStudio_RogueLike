using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickpocketsEnemy : Enemy
{    
    public Enemy enemy; // Enemy 实例引用
    public GameObject bulletPrefab; // 子弹的预制体
    public float bulletSpeed; // 子弹速度
    public bool bullet;
    public AttackEnemy attackEnemy;
    // 调用这个方法来尝试攻击
    public void TryAttack()
    {
        enemy=FindObjectOfType<Enemy>();
        Vector3 playerPosition = enemy.player.transform.position;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
        Vector2 bulletDirection = (playerPosition - gameObject.transform.position).normalized;
        attackEnemy=bulletInstance.GetComponent<AttackEnemy>();
        attackEnemy.enemy=enemy;
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
    }
    protected override void Awake()
    {
        base.Awake();

        patrolState = new PickpocketsStatePatrol(this, enemyFSM, this);
        chaseState = new PickpocketsStateChase(this, enemyFSM, this);
        attackState = new PickpocketsStateAttack(this, enemyFSM, this);
        deadState = new PickpocketsStateDead(this, enemyFSM, this);
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = patrolState;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
