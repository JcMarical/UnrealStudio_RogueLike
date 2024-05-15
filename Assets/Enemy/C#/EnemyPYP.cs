using UnityEngine;

public class EnemyPYP : MonoBehaviour
{
    // ����׷����ҵ�������
    public float chaseRange = 10f;
    // ���˽��й�������С����
    public float attackRange = 2f;
    // �����ƶ��ٶ�
    public float moveSpeed = 5f;
    // ������ת�ٶ�
    public float rotationSpeed = 5f;
    // ��������ֵ
    public int health = 100;

    // ��ҵ�Transform���
    private Transform player;
    // �����Ƿ��������ı�־
    private bool isDead = false;

    // ���˿��ܵ�״̬ö��
    private enum EnemyState
    {
        Idle,   // ����״̬
        Chase,  // ׷��״̬
        Attack, // ����״̬
        Hit,    // �ܻ�״̬
        Dead    // ����״̬
    }

    // ��ǰ���˵�״̬
    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        // ���Ҳ���ȡ��Ҷ����Transform���
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!isDead)
        {
            // ������������֮��ľ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ���ݵ�ǰ״ִ̬�в�ͬ���߼�
            switch (currentState)
            {
                case EnemyState.Idle:
                    // �����ҽ���׷��Χ���л���׷��״̬
                    if (distanceToPlayer <= chaseRange)
                    {
                        ChangeState(EnemyState.Chase);
                    }
                    break;
                case EnemyState.Chase:
                    // �����ҽ��빥����Χ���л�������״̬
                    if (distanceToPlayer <= attackRange)
                    {
                        AttackPlayer();
                    }
                    // �����ҳ���׷��Χ���л��ؿ���״̬
                    else if (distanceToPlayer > chaseRange)
                    {
                        ChangeState(EnemyState.Idle);
                    }
                    break;
            }
        }
    }

    // �л�����״̬�ķ���
    void ChangeState(EnemyState newState)
    {
        // ���Ҫ�л���״̬�뵱ǰ״̬��ͬ��ֱ�ӷ���
        if (currentState == newState)
            return;

        // ������״ִ̬����Ӧ����Ϊ
        switch (newState)
        {
            case EnemyState.Idle:
                // ֹͣ׷��͹�������
                break;
            case EnemyState.Chase:
                // ����׷�𶯻�
                break;
            case EnemyState.Attack:
                // ���Ź�������
                break;
            case EnemyState.Hit:
                // �����ܻ�����
                break;
            case EnemyState.Dead:
                // ��������������������������־Ϊtrue
                isDead = true;
                break;
        }

        // ���µ�ǰ״̬
        currentState = newState;
    }

    // ������ҵķ���
    void AttackPlayer()
    {
        // ������ʵ�ֹ����߼�
        // ������Ϻ��л���׷��״̬
        ChangeState(EnemyState.Chase);
    }

    // �����ܵ��˺��ķ���
    public void TakeDamage(int damage)
    {
        // ��ȥ����ֵ
        health -= damage;

        // �������ֵ�Ƿ�С�ڵ���0
        if (health <= 0)
        {
            // ���������δ�������л�������״̬
            if (!isDead)
            {
                ChangeState(EnemyState.Dead);
                // ������ʵ�������߼�
            }
        }
        else
        {
            // �������δ�������л����ܻ�״̬
            ChangeState(EnemyState.Hit);
        }
    }
}
