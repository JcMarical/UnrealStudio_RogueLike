using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 铜牌打手
/// </summary>
[CreateAssetMenu(fileName = "BronzeMedalStriker", menuName = "Data/Events/BronzeMedalStriker", order = 4)]
public class Event_BronzeMedalStriker : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        EventRoomMgr.Instance.EnterState(Event.BronzeMedalStriker);
    }

    public override void Choose1()
    {
        base.Choose1();

        Player.Instance.RealPlayerHealth -= 10;
        PropBackPackUIMgr.Instance.CurrenetCoins -= 5;
    }
}
