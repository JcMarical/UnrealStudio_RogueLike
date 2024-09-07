using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 诡异骰子
/// </summary>
[CreateAssetMenu(fileName = "WeirdDice", menuName = "Data/Events/WeirdDice", order = 0)]
public class Event_WeirdDiceEvent : EventData
{
    public override void Choose1()
    {
        base.Choose1();

        if (PropBackPackUIMgr.Instance.CurrenetDices < 1)
            EventRoomMgr.Instance.canContinue = false;
    }

    public override void Event0()
    {
        EventRoomMgr.Instance.DropCollection(EventRoomMgr.Instance.currentEvent.items[0], true);
    }

    public override void Event1()
    {
        PropBackPackUIMgr.Instance.CurrenetDices += 2;
    }

    public override void Event2()
    {
        
    }

    public override void Event3()
    {
        
    }
}
