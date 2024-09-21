using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MainPlayer;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChallengeRoom : MonoBehaviour 
{
    public GameObject[] EnemyPs; 
    [SerializeField]
    private GameObject[] normal;
    [SerializeField]
    private GameObject[] Elite;
    [SerializeField]
    private GameObject[] boss;

    private GameObject[] enemiesForSelect=new GameObject[3];
    private List<GameObject> enemiesForChallenge=new List<GameObject>();
    private int[] randoms=new int[3];
    private int isSelected=0;
    public int IsSelected{
        set{
            if(isSelected==0&&value!=0){
                
                Sequence mySequence=DOTween.Sequence();
                foreach(GameObject p in enemiesForSelect){
                    mySequence.Insert(0,DOTweenModuleSprite.DOFade(p.GetComponent<SpriteRenderer>(), 0, 2f)); 
                }
                mySequence.AppendCallback(()=>{
                    int random;
                    switch(value){
                        case 1:
                            random=UnityEngine.Random.Range(4,7);
                            InstantiateEnemy(random,normal[randoms[0]],EnemyPs[0].transform.position);
                            break;
                        case 2:
                            random=UnityEngine.Random.Range(3,5);
                            InstantiateEnemy(random,Elite[randoms[1]],EnemyPs[1].transform.position);
                            break;
                        case 3:
                            random=UnityEngine.Random.Range(1,3);
                            InstantiateEnemy(random,boss[randoms[2]],EnemyPs[2].transform.position);
                            break;
                    }
                    isSelected=value;
                });
                mySequence.Play();
            }
        }
        get{return isSelected;}
    }
    private void Update() {
        GameObject Target=null;
        if(GetCountOfList(enemiesForChallenge)==1&&IsSelected!=0){
            Target=enemiesForChallenge[0];
        }
        if(GetCountOfList(enemiesForChallenge)==0&&IsSelected!=0){
            switch(IsSelected){
                case 1:
                    PropDistributor.Instance.DistributeCoin(UnityEngine.Random.Range(3,6));
                    if(UnityEngine.Random.Range(0,10)<=9){
                        PropDistributor.Instance.DistributeCollection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer));
                    }
                    else{
                        PropDistributor.Instance.DistributeCollection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer+1));
                    }
                    break;
                case 2:
                    PropDistributor.Instance.DistributeCoin(UnityEngine.Random.Range(5,11));
                    if(UnityEngine.Random.Range(0,2)==0){
                        PropDistributor.Instance.DistributeCollection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer));
                    }
                    else{
                        PropDistributor.Instance.DistributeCollection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer+1));
                    }
                    if(UnityEngine.Random.Range(0,2)==0){
                        Instantiate(PropDistributor.Instance.DistributeRandomWeaponbyLevel(GameManager.Instance.CurrentLayer),Target.transform.position,Quaternion.identity);
                    }
                    break;
                case 3:
                    PropDistributor.Instance.DistributeCoin(UnityEngine.Random.Range(10,21));
                    PropDistributor.Instance.DistributeCollection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer+1));
                    if(UnityEngine.Random.Range(0,10)<=8){
                        Instantiate(PropDistributor.Instance.DistributeRandomWeaponbyLevel(GameManager.Instance.CurrentLayer),Target.transform.position,Quaternion.identity);
                    }
                    else{
                        Instantiate(PropDistributor.Instance.DistributeRandomWeaponbyLevel(GameManager.Instance.CurrentLayer+1),Target.transform.position,Quaternion.identity);
                    }
                    break;
            }
        }
    }
    public void RoomInitialization(){
        randoms[0]=UnityEngine.Random.Range(0,normal.Length);
        randoms[1]=UnityEngine.Random.Range(0,Elite.Length);
        randoms[2]=UnityEngine.Random.Range(0, boss.Length);

        enemiesForSelect[0]=new GameObject("1",typeof(CircleCollider2D),typeof(SpriteRenderer));
        enemiesForSelect[0].transform.position=EnemyPs[0].transform.position;
        enemiesForSelect[0].GetComponent<SpriteRenderer>().sprite=normal[randoms[0]].GetComponent<SpriteRenderer>().sprite;

        enemiesForSelect[1]=new GameObject("2",typeof(CircleCollider2D),typeof(SpriteRenderer));
        enemiesForSelect[1].transform.position=EnemyPs[1].transform.position;
        enemiesForSelect[1].GetComponent<SpriteRenderer>().sprite=Elite[randoms[1]].GetComponent<SpriteRenderer>().sprite; 

        enemiesForSelect[2]=new GameObject("3",typeof(CircleCollider2D),typeof(SpriteRenderer));
        enemiesForSelect[2].transform.position=EnemyPs[2].transform.position;
        enemiesForSelect[2].GetComponent<SpriteRenderer>().sprite=boss[randoms[2]].GetComponent<SpriteRenderer>().sprite;

        for(int i =0;i<3;i++){
            enemiesForSelect[i].GetComponent<Collider2D>().isTrigger=true;
            enemiesForSelect[i].AddComponent<EnemySelect>().enemyForSelect=new EnemyForSelect{
                challengeRoom=this,kind=i
            };
        }
    }
    private void InstantiateEnemy(int count,GameObject enemy,Vector3 pos){
        switch(count){
            case 1:
                enemiesForChallenge.Add(Instantiate(enemy,pos,Quaternion.identity));
                break;
            case 2:
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.right,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.left,Quaternion.identity));
                break;
            case 3:
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.left,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.right,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos,Quaternion.identity));
                break;
            case 4:
                enemiesForChallenge.Add(Instantiate(enemy,pos+new Vector3(1,1,0),Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+new Vector3(1,-1,0),Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+new Vector3(-1,1,0),Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+new Vector3(-1,-1,0),Quaternion.identity));
                break;
            case 5:
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.right,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.down,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.up,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.left,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos,Quaternion.identity));
                break;
            case 6:
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.left+Vector3.up,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.right+Vector3.up,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.up,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.left,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos+Vector3.right,Quaternion.identity));
                enemiesForChallenge.Add(Instantiate(enemy,pos,Quaternion.identity));
                break;
        }
    }
    private void Start() {
        RoomInitialization();
    }
    private int GetCountOfList<T>(List<T> l){
        int count = 0;
        foreach(T t in l){
            if(!t.Equals(null)){
                count++;
            }
        }
        return count;
    }
}
