using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    StoreRoomMgr Store;
    public Rigidbody2D Rigidbody;
    public float Velocity = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        Store = FindAnyObjectByType<StoreRoomMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Buy();
        }
    }

    void Move()
    {  
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Rigidbody.velocity = new Vector2(moveX, moveY).normalized * Velocity;
    }

    void Buy()
    {
        Store.BuyThings(Store.GetClosetGood(gameObject,StoreRoomMgr.Instance.Buy_Distance_Limit));
    }
}
