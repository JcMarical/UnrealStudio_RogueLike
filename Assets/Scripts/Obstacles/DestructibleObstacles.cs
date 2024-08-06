using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObstacles : MonoBehaviour
{
    public int getHit; //被打次数
    // Start is called before the first frame update
    void Start()
    {
        getHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
        getHit++;
    }
}
