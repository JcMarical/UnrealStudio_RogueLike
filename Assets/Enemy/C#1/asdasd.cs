using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdasd : MonoBehaviour
{
    public ObstaclesAndEnemyManager obstaclesAndEnemyManager;
    // Start is called before the first frame update
    void Start()
    {
        obstaclesAndEnemyManager=GetComponent<ObstaclesAndEnemyManager>();
        obstaclesAndEnemyManager.GenerateBossEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
