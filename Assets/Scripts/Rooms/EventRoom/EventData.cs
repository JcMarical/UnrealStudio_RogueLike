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

    public virtual void Choose0() => EventRoomMgr.Instance.choiceNumber = 0;
    public virtual void Choose1() => EventRoomMgr.Instance.choiceNumber = 1;
    public virtual void Choose2() => EventRoomMgr.Instance.choiceNumber = 2;
    public virtual void Choose3() => EventRoomMgr.Instance.choiceNumber = 3;
}

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
