using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无辜的羔羊
/// </summary>
[CreateAssetMenu(fileName = "InnocentLamb", menuName = "Data/Events/InnocentLamb", order = 2)]
public class InnocentLamb : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        EventRoomMgr.Instance.EnterState(Event.InnocentLamb);
    }

    public override void Choose1()
    {
        base.Choose1();

        //TODO: 给予玩家 “无辜的羔羊” 道具
    }

    public override void Choose2()
    {
        base.Choose2();

        Player.Instance.realLucky++;
    }
}
