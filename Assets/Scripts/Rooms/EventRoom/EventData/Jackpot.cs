using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 累计奖池
/// </summary>
[CreateAssetMenu(fileName = "Jackpot", menuName = "Data/Events/Jackpot", order = 5)]
public class Event_Jackpot : EventData
{
    public override void InitializeExtraWords()
    {
        base.InitializeExtraWords();

        EventRoomMgr.Instance.choiceExtraWords[0] = "（消耗" + 3 * GameManager.Instance.CurrentLayer + "个金币）" + EventRoomMgr.Instance.choiceExtraWords[0];
    }

    public override void Choose0()
    {
        base.Choose0();

        if (PropBackPackUIMgr.Instance.CurrenetCoins < 3 * GameManager.Instance.CurrentLayer)
            EventRoomMgr.Instance.canContinue = false;

        EventRoomMgr.Instance.rng = Random.Range(1, 4);

        EventRoomMgr.Instance.resultExtraWords = EventRoomMgr.Instance.rng switch
        {
            1 => "一个捣蛋玩具",
            2 => "一堆金闪闪的金币",
            3 => "一件亮眼的珍宝",
            4 => "一团空气",
            _ => ""
        };

        EventRoomMgr.Instance.endTitleExtraWords = EventRoomMgr.Instance.rng switch
        {
            1 => "真倒霉！",
            2 => "运气不错！",
            3 => "运气不错！",
            4 => "真倒霉！",
            _ => ""
        };

        EventRoomMgr.Instance.endDescriptionExtraWords = EventRoomMgr.Instance.rng switch
        {
            1 => "生命值减少" + 0.5 * GameManager.Instance.CurrentLayer + "颗心",
            2 => "金币加" + 6 * GameManager.Instance.CurrentLayer,
            3 => "随机获取一件稀有度为" + GameManager.Instance.CurrentLayer + "的道具",
            4 => "运气减一",
            _ => ""
        };
    }

    public override void Event0()
    {
        switch (EventRoomMgr.Instance.rng)
        {
            case 1:
                Player.Instance.RealPlayerHealth -= 5 * GameManager.Instance.CurrentLayer;
                break;
            case 2:
                PropBackPackUIMgr.Instance.CurrenetCoins += 6 * GameManager.Instance.CurrentLayer;
                break;
            case 3:
                EventRoomMgr.Instance.DropProp(GameManager.Instance.CurrentLayer);
                break;
            case 4:
                Player.Instance.RealLucky--;
                break;
            default:
                break;
        }
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
