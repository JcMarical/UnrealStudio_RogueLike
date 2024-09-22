using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{ 
    None,
    Buy,
    GetThings_ObjectRoom,
    TalktoBoss_StoreRoom,
}

public class PlayerInterAct : TInstance<PlayerInterAct>
{
    private void Awake()
    {
        base.Awake();
    }

    public InteractType interactType = InteractType.None;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            switch (interactType)
            {
                case InteractType.None:
                    break;

                case InteractType.Buy:
                    if (StoreRoomMgr.Instance.GetClosetGood(gameObject, StoreRoomMgr.Instance.Buy_Distance_Limit) != null)
                    {
                        StoreRoomMgr.Instance.BuyThings(StoreRoomMgr.Instance.GetClosetGood(gameObject, StoreRoomMgr.Instance.Buy_Distance_Limit));
                    }
                    break;

                case InteractType.GetThings_ObjectRoom:
                    ObjectRoomMgr.Instance.GetObject(gameObject);
                    break;

                case InteractType.TalktoBoss_StoreRoom:
                    StoreRoomMgr.Instance.TalkToBoss();
                    break;
            }
        }
    }
}
