using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
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
                            transform.Translate(speed * Time.deltaTime * (Target.transform.position-transform.position).normalized);
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
}
