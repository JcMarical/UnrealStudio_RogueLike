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
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Buy();
            GetObject();
        }
        //Move();
    }

    void Move()
    { 
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Rigidbody.velocity = new Vector2(inputX * Velocity, inputY * Velocity);
    }

    void Buy()
    {
        if(Store.GetClosetGood(gameObject, StoreRoomMgr.Instance.Buy_Distance_Limit) != null)
        Store.BuyThings(Store.GetClosetGood(gameObject,StoreRoomMgr.Instance.Buy_Distance_Limit));
    }

    void GetObject()
    {
        ObjectRoomMgr.Instance.GetObject(gameObject);
    }
}
