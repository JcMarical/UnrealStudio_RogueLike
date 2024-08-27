using MainPlayer;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using static Enemy;

public class BigGuyBrain : MonoBehaviour
{
    bool isPlayer=true;
    public GameObject Target;
    public float speed=2f;
    private Action UpdateAction;
    private Action FixedUpdateAction;
    private float time=0;
    private int _state=0;
    private int State{
        set{
            switch(value){
                //追踪
                case 0:
                    FixedUpdateAction+=()=>{
                        if (Target != null){
                            //transform.Translate(speed * Time.deltaTime * (Target.transform.position-transform.position).normalized);
                            AutoPath();
                            ChaseMove();
                        }
                    };
                    UpdateAction=null;
                    _state=0;
                    break;
                //攻击
                case 1:
                    UpdateAction+=()=>{
                        time+=Time.deltaTime;
                        if(time>=2){
                            Target.GetComponent<Enemy>().GetHit(10);
                            time=0;
                        }
                    };
                    FixedUpdateAction=null;
                    _state=1;
                    break;
                //待机
                case 2:
                    FixedUpdateAction=null;
                    UpdateAction=null;
                    _state=2;
                    break;
            }
        }
        get{
            return _state;
        }
    }
    private void Start() {
        Target=MainPlayer.Player.Instance.gameObject;
        State=0;
        WeaponCtrl.Instance.OnDamage+=OnDamage;
        seeker=GetComponent<Seeker>();
    }
    private void Update() {
        UpdateAction?.Invoke();
        if((Target.transform.position-transform.position).magnitude<ConstField.Instance.DeviationOfVelocity){
            if(isPlayer&&State!=2){
                State=2;
            }
            if(!isPlayer&&State!=1){
                time=0;
                State=1;
            }
        }
        else{
            if(State!=0){
                State=0;
            }
        }
    }
    private void FixedUpdate() {
        FixedUpdateAction?.Invoke();
    }
    private void OnDamage(GameObject Enemy){
        if(Enemy.GetComponent<Enemy>().enemyQuality==EnemyQuality.normal){
            Target=Enemy;
            isPlayer=false;
        }
    }

    #region 自动寻路

    private Seeker seeker;
    private List<Vector3> pathPointList;
    private int currentIndex = 0;
    private float pathFindingTime = 0.5f;
    private float pathFindingTimer = 0f;
    public Vector2 moveDirection;

    private void PathFinding(Vector3 target)  //获取路径点
    {
        currentIndex = 0;
        //三个参数：起点，终点，回调函数
        seeker.StartPath(transform.position, target, Path =>
        {
            pathPointList = Path.vectorPath;
        });
    }

    public void AutoPath()  //自动寻路
    {
        pathFindingTimer += Time.deltaTime;

        //每0.5s调用一次路径生成函数
        if (pathFindingTimer > pathFindingTime)
        {
            PathFinding(Target.transform.position);
            pathFindingTimer = 0f;
        }


        if (pathPointList == null || pathPointList.Count <= 0)  //为空则获取路径点
        {
            PathFinding(Target.transform.position);
        }
        else if (Vector2.Distance(transform.position, pathPointList[currentIndex]) <= 0.1f)
        {
            currentIndex += 1;
            if (currentIndex >= pathPointList.Count)
            {
                PathFinding(Target.transform.position);
            }
        }

        if (pathPointList != null)
        {
            moveDirection = (pathPointList[currentIndex] - transform.position).normalized;
        }
    }

    public void ChaseMove()
    {
        if (pathPointList != null && pathPointList.Count > 0 && currentIndex >= 0 && currentIndex < pathPointList.Count)
        {
            transform.Translate(moveDirection * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime * (Target.transform.position-transform.position).normalized);
        }
    }

    #endregion
}
