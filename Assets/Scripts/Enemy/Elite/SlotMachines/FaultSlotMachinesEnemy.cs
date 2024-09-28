using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Xml;
using UnityEngine;

public class FaultSlotMachinesEnemy : Enemy
{
    public PropDistributor propDistributor;
    public Enemy enemy; // Enemy 实例引用
    public AttackEnemy attackEnemy;
    public GameObject bulletPrefab; // 子弹的预制体
    public float bulletSpeed; // 子弹速度
    public bool bullet;
    // 调用这个方法来尝试攻击
    public void TryAttack()
    {
        enemy = FindObjectOfType<Enemy>();
        Vector3 playerPosition = enemy.player.transform.position;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
        attackEnemy=bulletInstance.GetComponent<AttackEnemy>();
        attackEnemy.enemy = enemy;
        Vector2 bulletDirection = (playerPosition - gameObject.transform.position).normalized;
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
    }

    public void FinishAttack()
    {
        enemyFSM.ChangeState(patrolState);
    }
    protected override void Awake()
    {
        base.Awake();

        patrolState = new FaultSlotMachinesStatePatrol(this, enemyFSM, this);
        chaseState = new FaultSlotMachinesStateChase(this, enemyFSM, this);
        attackState = new FaultSlotMachinesStateAttack(this, enemyFSM, this, (FaultSlotMachinesStatePatrol)patrolState);
        deadState = new FaultSlotMachinesStateDead(this, enemyFSM, this);
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
        if (currentHealth <= 0)
        {
            propDistributor.DistributeCoin(Random.Range(coinNumber.min, coinNumber.max + 1));
            enemyFSM.ChangeState(deadState);
            return;
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
