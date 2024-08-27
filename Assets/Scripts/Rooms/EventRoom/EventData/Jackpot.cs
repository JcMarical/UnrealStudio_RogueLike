using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 累计奖池
/// </summary>
[CreateAssetMenu(fileName = "Jackpot", menuName = "Data/Events/Jackpot", order = 5)]
public class Jackpot : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        int rng = Random.Range(1, 4);
        switch (rng)
        {
            case 1:
                Player.Instance.realPlayerHealth -= 5 * GameManager.Instance.CurrentLayer;
                break;
            case 2:
                PropBackPackUIMgr.Instance.CurrenetCoins += 6 * GameManager.Instance.CurrentLayer;
                break;
            case 3:
                //TODO: 玩家随机获得一件稀有度为当前层的道具
                break;
            case 4:
                Player.Instance.realLucky -= 1 * GameManager.Instance.CurrentLayer;
                break;
            default:
                break;
        }
    }

    public override void Choose1()
    {
        base.Choose1();
    }
}
