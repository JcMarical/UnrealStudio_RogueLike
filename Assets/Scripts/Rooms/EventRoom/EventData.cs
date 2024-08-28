using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventData : ScriptableObject
{
    [Serializable]
    public struct Choice
    {
        [Tooltip("选项名称")] public string title;
        [Tooltip("选项描述")][TextArea(2, 4)] public string description;
        [Tooltip("选项结果")][TextArea(2, 4)] public string result;
    }

    [Tooltip("背景图片")] public Image backgroundImage;
    [Tooltip("事件标题")] public string eventTitle;
    [Tooltip("事件描述")][TextArea(3, 6)] public string eventDescription;
    [Tooltip("选项")] public Choice[] choices;
    [Tooltip("事件能否重复出现")] public bool isRepeatable;
    [Tooltip("事件出现的层数（0代表不限层数）")] public int layer;

    public virtual void Choose0() => EventRoomMgr.Instance.choiceNumber = 0;
    public virtual void Choose1() => EventRoomMgr.Instance.choiceNumber = 1;
    public virtual void Choose2() => EventRoomMgr.Instance.choiceNumber = 2;
    public virtual void Choose3() => EventRoomMgr.Instance.choiceNumber = 3;
}

/// <summary>
/// 诡异骰子
/// </summary>
[CreateAssetMenu(fileName = "WeirdDice", menuName = "Data/Events/WeirdDice", order = 0)]
public class WeirdDiceEvent : EventData
{
    public override void Choose0()
    {
        base.Choose0();
    }

    public override void Choose1()
    {
        base.Choose1();
    }

    public override void Choose2()
    {
        base.Choose2();
    }
}

/// <summary>
/// 射击游戏
/// </summary>
[CreateAssetMenu(fileName = "ShootingGame", menuName = "Data/Events/ShootingGame", order = 1)]
public class ShootingGame : EventData
{
    public override void Choose0()
    {
        base.Choose0();
    }

    public override void Choose1()
    {
        base.Choose1();
    }

    public override void Choose2()
    {
        base.Choose2();
    }
}

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
    }

    public override void Choose2()
    {
        base.Choose2();
    }
}

/// <summary>
/// 嘿！大块头！
/// </summary>
[CreateAssetMenu(fileName = "HeftyMan", menuName = "Data/Events/HeftyMan", order = 3)]
public class HeftyMan: EventData
{
    public override void Choose0()
    {
        base.Choose0();
    }

    public override void Choose1()
    {
        base.Choose1();
    }
}

/// <summary>
/// 铜牌打手
/// </summary>
[CreateAssetMenu(fileName = "BronzeMedalStriker", menuName = "Data/Events/BronzeMedalStriker", order = 4)]
public class BronzeMedalStriker : EventData
{
    public override void Choose0()
    {
        base.Choose0();
    }

    public override void Choose1()
    {
        base.Choose1();
    }
}

/// <summary>
/// 累计奖池
/// </summary>
[CreateAssetMenu(fileName = "Jackpot", menuName = "Data/Events/Jackpot", order = 5)]
public class Jackpot : EventData
{
    public override void Choose0()
    {
        base.Choose0();
    }

    public override void Choose1()
    {
        base.Choose1();
    }
}

/// <summary>
/// 意外之喜
/// </summary>
[CreateAssetMenu(fileName = "HappyAccident", menuName = "Data/Events/HappyAccident", order = 6)]
public class HappyAccident: EventData
{
    public override void Choose0()
    {
        base.Choose0();
    }
}
