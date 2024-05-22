using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPYP : MonoBehaviour
{
    #region �Զ�Ѱ·
    public GameObject player;
    public float chaseSpeed = 3f;
    private Seeker seeker;
    private List<Vector3> pathPointList;
    private int currentIndex = 0;
    private float pathFindingTime = 0.5f;
    private float pathFindingTimer = 0f;

    private void PathFinding(Vector3 target)  //��ȡ·����
    {
        currentIndex = 0;
        //������������㣬�յ㣬�ص�����
        seeker.StartPath(transform.position, target, Path =>
        {
            pathPointList = Path.vectorPath;
        });
    }

    private void AutoPath()  //�Զ�Ѱ·
    {
        pathFindingTimer += Time.deltaTime;
        
        //ÿ0.5s����һ��·�����ɺ���
        if (pathFindingTimer > pathFindingTime)
        {
            PathFinding(player.transform.position);
            pathFindingTimer = 0f;
        }


        if (pathPointList == null || pathPointList.Count <= 0)  //Ϊ�����ȡ·����
        {
            PathFinding(player.transform.position);
        }
        else if (Vector2.Distance(transform.position, pathPointList[currentIndex]) <= 0.1f)
        {
            currentIndex += 1;
            if (currentIndex >= pathPointList.Count)
            {
                PathFinding(player.transform.position);
            }
        }
    }
     
    public void ChaseMove2()
    {
        Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //��·���㷽��
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
    }

    #endregion

    public void Start()
    {
        seeker= GetComponent<Seeker>();
    }
    public void Update()
    {
        if (player != null)
        {
            AutoPath(); // ����·����
            pathFindingTimer = pathFindingTime; // ���ü�ʱ��
            ChaseMove2();
        }
    }

}

