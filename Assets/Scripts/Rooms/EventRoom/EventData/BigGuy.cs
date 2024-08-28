using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 嘿！大块头！
/// </summary>
[CreateAssetMenu(fileName = "BigGuy", menuName = "Data/Events/BigGuy", order = 3)]
public class Event_BigGuy : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        EventRoomMgr.Instance.DropCollection(EventRoomMgr.Instance.currentEvent.items[0], true);
    }

    public override void Choose1()
    {
        base.Choose1();
    }
}
