using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletP : MonoBehaviour
{
    float time;
    public float bulletTime;
    public bool pierceWall;  //ÊÇ·ñ´©Ç½
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>bulletTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!pierceWall)
        {
            if(other.CompareTag("Obstacles"))
            {
                Destroy(gameObject);
            }
        }
    }
}
