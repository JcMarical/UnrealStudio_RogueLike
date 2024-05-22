using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPYP : MonoBehaviour
{
    #region 自动寻路
    public GameObject player;
    public float chaseSpeed = 3f;
    private Seeker seeker;
    private List<Vector3> pathPointList;
    private int currentIndex = 0;
    private float pathFindingTime = 0.5f;
    private float pathFindingTimer = 0f;

    private void PathFinding(Vector3 target)  //获取路径点
    {
        currentIndex = 0;
        //三个参数：起点，终点，回调函数
        seeker.StartPath(transform.position, target, Path =>
        {
            pathPointList = Path.vectorPath;
        });
    }

    private void AutoPath()  //自动寻路
    {
        pathFindingTimer += Time.deltaTime;
        
        //每0.5s调用一次路径生成函数
        if (pathFindingTimer > pathFindingTime)
        {
            PathFinding(player.transform.position);
            pathFindingTimer = 0f;
        }


        if (pathPointList == null || pathPointList.Count <= 0)  //为空则获取路径点
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
        Vector2 direction = (pathPointList[currentIndex] - transform.position).normalized; //沿路径点方向
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
            AutoPath(); // 更新路径点
            pathFindingTimer = pathFindingTime; // 重置计时器
            ChaseMove2();
        }
    }

}

