using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EventData : ScriptableObject
{
    [Serializable]
    public struct Choice
    {
        [Tooltip("选项名称")] public string title;
        [Tooltip("选项描述")][TextArea(2, 4)] public string description;
        [Tooltip("有无风险")] public bool isWarning;
        [Space(16)]
        [Tooltip("选项结果")][TextArea(3, 6)] public string result;
        [Space(16)]
        [Tooltip("结束标题")] public string endTitle;
        [Tooltip("结束描述")][TextArea(2, 4)] public string endDesctiption;
    }

    [Tooltip("背景图片")] public Image backgroundImage;
    [Tooltip("事件标题")] public string eventTitle;
    [Tooltip("事件描述")][TextArea(3, 6)] public string eventDescription;
    [Tooltip("选项")] public Choice[] choices;
    [Tooltip("事件能否重复出现")] public bool isRepeatable;
    [Tooltip("事件出现的层数（0代表不限层数）")] public int layer;
    [Space(16)]
    [Tooltip("敌人")] public GameObject[] enemys;
    [Tooltip("物品")] public ObtainableObjectData[] items;
    [Tooltip("武器")] public GameObject[] weapons;

    protected string Warning
    {
        get => GameManager.Instance.Unease > 0 ? "（<color=red>风险的预演</color>）" : null;
    }

    public virtual void Choose0()
    {
        EventRoomMgr.Instance.choiceNumber = 0;
        EventRoomMgr.Instance.canContinue = true;
    }
    
    public virtual void Choose1()
    {
        EventRoomMgr.Instance.choiceNumber = 1;
        EventRoomMgr.Instance.canContinue = true;
    }
    
    public virtual void Choose2()
    {
        EventRoomMgr.Instance.choiceNumber = 2;
        EventRoomMgr.Instance.canContinue = true;
    }
    
    public virtual void Choose3()
    {
        EventRoomMgr.Instance.choiceNumber = 3;
        EventRoomMgr.Instance.canContinue = true;
    }

    public abstract void Event0();
    public abstract void Event1();
    public abstract void Event2();
    public abstract void Event3();

    public virtual void InitializeExtraWords() 
    {
        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i].isWarning)
                EventRoomMgr.Instance.choiceExtraWords[i] = Warning;
            else
                EventRoomMgr.Instance.choiceExtraWords[i] = "";
        }

        EventRoomMgr.Instance.resultExtraWords = "";
        EventRoomMgr.Instance.endTitleExtraWords = "";
        EventRoomMgr.Instance.endDescriptionExtraWords = "";
    }
}