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
    public override void Event0()
    {
        Player.Instance.RealPlayerHealth -= 10;
        PropBackPackUIMgr.Instance.CurrenetCoins += 5;
    }

    public override void Event1()
    {
        PropBackPackUIMgr.Instance.CurrenetCoins += 5;
        GameManager.Instance.Unease += 5;
    }

    public override void Event2()
    {
        
    }

    public override void Event3()
    {
        
    }
}
