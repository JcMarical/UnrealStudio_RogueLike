using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 射击游戏
/// </summary>
[CreateAssetMenu(fileName = "ShootingGame", menuName = "Data/Events/ShootingGame", order = 1)]
public class Event_ShootingGame : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        Player.Instance.RealPlayerHealth -= 10;
        PropBackPackUIMgr.Instance.CurrenetCoins += 5;
    }

    public override void Choose1()
    {
        base.Choose1();

        PropBackPackUIMgr.Instance.CurrenetCoins += 5;
        Player.Instance.RealUnlucky += 5;
    }

    public override void Choose2()
    {
        base.Choose2();
    }
}
