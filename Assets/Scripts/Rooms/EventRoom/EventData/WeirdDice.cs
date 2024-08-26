using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 诡异骰子
/// </summary>
[CreateAssetMenu(fileName = "WeirdDice", menuName = "Data/Events/WeirdDice", order = 0)]
public class WeirdDiceEvent : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        //TODO: 给予玩家 “怪异骰子” 藏品
    }

    public override void Choose1()
    {
        base.Choose1();

        if (PropBackPackUIMgr.Instance.CurrenetDices > 0)
            PropBackPackUIMgr.Instance.CurrenetDices += 2;
        else
            EventRoomMgr.Instance.canContinue = false;
    }

    public override void Choose2()
    {
        base.Choose2();
    }
}
