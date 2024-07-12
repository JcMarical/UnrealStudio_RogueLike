using UnityEngine;
using static Enemy;
/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class PickpocketsStatePatrol : EnemyState
{
    PickpocketsEnemy pickpocketsEnemy;
    private float timer;
    private Vector2 patrolDirection;
    private float[] patrolTime = { 0.6f, 1f, 1.4f };
    private bool isPatrol;
    private float attackTime;

    public PickpocketsStatePatrol(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {
        this.pickpocketsEnemy = pickpocketsEnemy;
    }

    public override void OnEnter()
    {
        timer = 1;
        isPatrol = false;
        attackTime = 0f;
    }

    public override void LogicUpdate()
    {
        if (pickpocketsEnemy.bullet)
        {
            attackTime = 10f;
        }
        else
        {
            Debug.Log(attackTime);
            if (attackTime > 0f)
            {
                attackTime -= Time.deltaTime;
            }
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0 && isPatrol)
            {
                timer = 1;
                isPatrol = false;
            }
            else
            {
                int i = Random.Range(0, patrolTime.Length);
                timer = patrolTime[i];  // 随机巡逻时间
                float patrolDistance = enemy.patrolSpeed * patrolTime[i];   // 计算巡逻距离

                do
                {
                    patrolDirection = Random.insideUnitCircle; // 随机选择一个方向进行巡逻
                    patrolDirection.Normalize(); // 将方向向量归一化
                } while (Physics2D.Raycast(enemy.transform.position, patrolDirection, patrolDistance, enemy.obstacleLayer));    // 当巡逻路线上有障碍物时重新随机巡逻方向

                isPatrol = true;
            }
        }

        if (enemy.IsPlayerInVisualRange())
        {
            if (attackTime <= 0f && !pickpocketsEnemy.bullet)
            {
                pickpocketsEnemy.TryAttack();
                attackTime = 1.5f;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (!pickpocketsEnemy.bullet)
        {
            if (isPatrol)
            {
                // 在这里可能需要播放移动动画
                enemy.Move(patrolDirection, enemy.patrolSpeed);
            }
            else
            {
                // 在这里可能需要播放停止动画
            }
        }
    }

    public override void OnExit()
    {
        // 在这里可能需要播放停止动画
    }
}


/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
/// </summary>
public class PickpocketsStateChase : EnemyState
{
    private float coolDownTimer;
    private float hatredTimer;
    private Vector2 chaseDirection;
    private Vector2 retreatDirection;

    public PickpocketsStateChase(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isMove", true);  //播放跑的动画

        coolDownTimer = enemy.globalTimer;
        hatredTimer = 2;
        chaseDirection = (enemy.player.transform.position - enemy.transform.position).normalized;
    }

    public override void LogicUpdate()
    {
        //切换到攻击状态的逻辑判断
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }
        else
        {
            if (enemy.IsPlayerInVisualRange())
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
            if (enemy.IsPlayerInVisualRange() && !enemy.IsPlayerInAttackRange())
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
        }
        //丢失仇恨切换到巡逻状态的逻辑判断
        if (!enemy.IsPlayerInVisualRange())
        {
            hatredTimer -= Time.deltaTime;
        }
        else
        {
            hatredTimer = 5;
        
        }

        if (hatredTimer <= 0)
        {
            enemyFSM.ChangeState(enemy.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        //enemy.anim.SetBool("isMove", false);
    }
}

/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
/// </summary>
public class PickpocketsStateAttack : EnemyState
{
    private GameObject projectilePrefab; // 发射物预制体

    public PickpocketsStateAttack(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}

/// <summary>
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
/// </summary>
public class PickpocketsStateDead : EnemyState
{
    public PickpocketsStateDead(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        //enemy.anim.SetBool("isDead", true);
        enemy.gameObject.layer = 2;
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        enemy.rb.velocity = Vector2.zero;
    }

    public override void OnExit()
    {

    }
}
