using UnityEngine;

public class EnemyPYP : MonoBehaviour,IDamageable
{
    public void GetHit(float damage)
    {

    }

    public void Repelled(float force)
    {

    }
    // 敌人追逐玩家的最大距离
    public float chaseRange = 10f;
    // 敌人进行攻击的最小距离
    public float attackRange = 2f;
    // 敌人移动速度
    public float moveSpeed = 5f;
    // 敌人旋转速度
    public float rotationSpeed = 5f;
    // 敌人生命值
    public int health = 100;

    // 玩家的Transform组件
    private Transform player;
    // 敌人是否已死亡的标志
    private bool isDead = false;

    // 敌人可能的状态枚举
    private enum EnemyState
    {
        Idle,   // 空闲状态
        Chase,  // 追逐状态
        Attack, // 攻击状态
        Hit,    // 受击状态
        Dead    // 死亡状态
    }

    // 当前敌人的状态
    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        // 查找并获取玩家对象的Transform组件
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!isDead)
        {
            // 计算敌人与玩家之间的距离
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 根据当前状态执行不同的逻辑
            switch (currentState)
            {
                case EnemyState.Idle:
                    // 如果玩家进入追逐范围，切换到追逐状态
                    if (distanceToPlayer <= chaseRange)
                    {
                        ChangeState(EnemyState.Chase);
                    }
                    break;
                case EnemyState.Chase:
                    // 如果玩家进入攻击范围，切换到攻击状态
                    if (distanceToPlayer <= attackRange)
                    {
                        AttackPlayer();
                    }
                    // 如果玩家超出追逐范围，切换回空闲状态
                    else if (distanceToPlayer > chaseRange)
                    {
                        ChangeState(EnemyState.Idle);
                    }
                    break;
            }
        }
    }

    // 切换敌人状态的方法
    void ChangeState(EnemyState newState)
    {
        // 如果要切换的状态与当前状态相同，直接返回
        if (currentState == newState)
            return;

        // 根据新状态执行相应的行为
        switch (newState)
        {
            case EnemyState.Idle:
                // 停止追逐和攻击动画
                break;
            case EnemyState.Chase:
                // 播放追逐动画
                break;
            case EnemyState.Attack:
                // 播放攻击动画
                break;
            case EnemyState.Hit:
                // 播放受击动画
                break;
            case EnemyState.Dead:
                // 播放死亡动画，并设置死亡标志为true
                isDead = true;
                break;
        }

        // 更新当前状态
        currentState = newState;
    }

    // 攻击玩家的方法
    void AttackPlayer()
    {
        // 在这里实现攻击逻辑
        // 攻击完毕后切换回追逐状态
        ChangeState(EnemyState.Chase);
    }

    // 敌人受到伤害的方法
    public void TakeDamage(int damage)
    {
        // 减去生命值
        health -= damage;

        // 检查生命值是否小于等于0
        if (health <= 0)
        {
            // 如果敌人尚未死亡，切换到死亡状态
            if (!isDead)
            {
                ChangeState(EnemyState.Dead);
                // 在这里实现死亡逻辑
            }
        }
        else
        {
            // 如果敌人未死亡，切换到受击状态
            ChangeState(EnemyState.Hit);
        }
    }
}
