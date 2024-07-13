using UnityEngine;
using static Enemy;
/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class PickpocketsStatePatrol : EnemyState
{
    PickpocketsEnemy pickpocketsEnemy;
    protected float basicMoveTime;
    protected float currentMoveTime;
    protected float moveTimer;
    protected float waitTimer;
    protected float moveAngle;
    protected Vector2 moveDirection;
    public float attackTime;

    public PickpocketsStatePatrol(Enemy enemy, EnemyFSM enemyFSM, PickpocketsEnemy pickpocketsEnemy) : base(enemy, enemyFSM)
    {
        this.pickpocketsEnemy = pickpocketsEnemy;
    }

    public override void OnEnter()
    {
        attackTime = 0f;
        moveAngle = Random.Range(0, 360);
        moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;
        basicMoveTime = enemy.basicPatrolDistance / enemy.patrolSpeed;
        currentMoveTime = Random.Range(enemy.basicPatrolDistance - 1, enemy.basicPatrolDistance + 1) / enemy.patrolSpeed;
        moveTimer = currentMoveTime;
        waitTimer = enemy.patrolWaitTime;
    }

    public override void LogicUpdate()
    {
        if(attackTime>0f)
            attackTime-=Time.deltaTime;

        if (waitTimer >= 0 && !enemy.isPatrolMove)
            waitTimer -= Time.deltaTime;

        if (waitTimer < 0)
            enemy.isPatrolMove = true;

        if (moveTimer > 0 && enemy.isPatrolMove)
            moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            enemy.isPatrolMove = false;

            moveAngle = Random.Range(moveAngle + 120, moveAngle + 240);
            moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;

            if (currentMoveTime > basicMoveTime * 2 || currentMoveTime < basicMoveTime * 0.5f)
                currentMoveTime = basicMoveTime;
            else
                currentMoveTime *= Random.Range(0.75f, 1.5f);
            moveTimer = currentMoveTime;

            waitTimer = enemy.patrolWaitTime;
        }

        if (enemy.isPatrolMove && enemy.isCollideWall)
        {
            enemy.isPatrolMove = false;
            enemy.isCollideWall = false;

            switch (enemy.collideDirection)
            {
                case 1:
                    moveAngle = Random.Range(-60, 60); break;
                case 2:
                    moveAngle = Random.Range(30, 150); break;
                case 3:
                    moveAngle = Random.Range(120, 240); break;
                case 4:
                    moveAngle = Random.Range(210, 330); break;
                default:
                    moveAngle = Random.Range(0, 360); break;
            }
            moveDirection = Quaternion.Euler(0, 0, moveAngle) * Vector2.right;

            if (currentMoveTime > basicMoveTime * 2 || currentMoveTime < basicMoveTime * 0.5f)
                currentMoveTime = basicMoveTime;
            else
                currentMoveTime *= Random.Range(0.75f, 1.5f);
            moveTimer = currentMoveTime;

            waitTimer = enemy.patrolWaitTime;
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
        if (enemy.isPatrolMove)
            enemy.Move(moveDirection, enemy.patrolSpeed);
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
