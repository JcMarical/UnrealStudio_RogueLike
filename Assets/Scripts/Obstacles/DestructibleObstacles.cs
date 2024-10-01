using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObstacles : MonoBehaviour
{
    public int getHit; //被打次数
    private float coolDownTime;
    // Start is called before the first frame update
    void Start()
    {
        getHit = 0;
        coolDownTime=0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(coolDownTime>0){
            coolDownTime-=Time.deltaTime;
        }
        if (getHit >= 5)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 攻击到了调用该函数
    /// </summary>
    public void hit()
    {
        if(coolDownTime<0){
            getHit++;
            coolDownTime=0.2f;
        }
    }
}
