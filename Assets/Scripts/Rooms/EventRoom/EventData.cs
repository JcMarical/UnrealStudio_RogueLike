using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EventData", menuName = "Data/Rooms/EventRoom", order = 1)]
public class EventData : ScriptableObject
{
    [Serializable]
    public struct Choice
    {
        [Tooltip("选项名称")] public string title;
        [Tooltip("选项描述")][TextArea(2, 4)] public string description;
    }

    [Tooltip("背景图片")] public Image backgroundImage;
    [Tooltip("事件标题")] public string eventTitle;
    [Tooltip("事件描述")][TextArea(3, 6)] public string eventDescription;
    [Tooltip("选项")] public Choice[] choices;
    [Tooltip("事件能否重复出现")] public bool isRepeatable;
}
