using System.Collections;
using System.Collections.Generic;
using MainPlayer;
using Sirenix.Utilities;
using UnityEngine;

public class ChallengeRoom : Room
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
                foreach(GameObject p in enemiesForSelect){
                    Destroy(p);
                }
                switch(value){
                    case 1:
                        Instantiate(normal[randoms[0]],EnemyPs[0].transform.position,Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(Elite[randoms[1]],EnemyPs[1].transform.position,Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(boss[randoms[2]],EnemyPs[2].transform.position,Quaternion.identity);
                        break;
                }
                isSelected=value;
            }
        }
        get{return isSelected;}
    }
    private void Update() {
        GameObject Target=null;
        if(enemiesForChallenge.Count==1&&IsSelected!=0){
            Target=enemiesForChallenge[0];
        }
        if(enemiesForChallenge.IsNullOrEmpty()&&IsSelected!=0){
            switch(IsSelected){
                case 1:
                    PropDistributor.Instance.DistributeCoin(Random.Range(3,6));
                    if(Random.Range(0,10)<=9){
                        PropDistributor.Instance.DistributeColection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer));
                    }
                    else{
                        PropDistributor.Instance.DistributeColection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer+1));
                    }
                    break;
                case 2:
                    PropDistributor.Instance.DistributeCoin(Random.Range(5,11));
                    if(Random.Range(0,2)==0){
                        PropDistributor.Instance.DistributeColection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer));
                    }
                    else{
                        PropDistributor.Instance.DistributeColection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer+1));
                    }
                    if(Random.Range(0,2)==0){
                        Instantiate(PropDistributor.Instance.DistributeRandomWeaponbyLevel(GameManager.Instance.CurrentLayer),Target.transform.position,Quaternion.identity);
                    }
                    break;
                case 3:
                    PropDistributor.Instance.DistributeCoin(Random.Range(10,21));
                    PropDistributor.Instance.DistributeColection(Target.transform.position,Player.Instance.gameObject.transform.position,PropDistributor.Instance.DistributeRandomCollectionbyLevel(GameManager.Instance.CurrentLayer+1));
                    if(Random.Range(0,10)<=8){
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
        randoms[0]=Random.Range(0,normal.Length);
        randoms[1]=Random.Range(0,Elite.Length);
        randoms[2]=Random.Range(0, boss.Length);
        enemiesForSelect[0]=Instantiate(normal[randoms[0]],EnemyPs[0].transform.position,Quaternion.identity);
        enemiesForSelect[1]=Instantiate(Elite[randoms[1]],EnemyPs[1].transform.position,Quaternion.identity);
        enemiesForSelect[2]=Instantiate(boss[randoms[2]],EnemyPs[2].transform.position,Quaternion.identity);
        for(int i =0;i<3;i++){
            Destroy(enemiesForSelect[i].GetComponent<Enemy>());
            enemiesForSelect[i].GetComponent<Collider2D>().isTrigger=true;
            enemiesForSelect[i].AddComponent<EnemySelect>().enemyForSelect=new EnemyForSelect{
                challengeRoom=this,kind=i
            };
        }
    }
}
