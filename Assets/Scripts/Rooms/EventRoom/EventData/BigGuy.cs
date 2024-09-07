using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 嘿！大块头！
/// </summary>
[CreateAssetMenu(fileName = "BigGuy", menuName = "Data/Events/BigGuy", order = 3)]
public class Event_BigGuy : EventData
{
    public override void Event0()
    {
        EventRoomMgr.Instance.DropCollection(EventRoomMgr.Instance.currentEvent.items[0], true);
    }

    public override void Event1()
    {
        
    }

    public override void Event2()
    {
        
    }

    public override void Event3()
    {
        
    }
}
