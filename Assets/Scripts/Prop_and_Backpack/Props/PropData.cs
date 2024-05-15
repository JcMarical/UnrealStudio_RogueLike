using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="PropData",menuName ="Data/Props/Data",order =1)]
public class PropData : ScriptableObject
{
    /// <summary>
    /// 道具稀有度，从N到UR稀有度依次递增，N对应1，以此类推
    /// </summary>
    public enum Rarities
    { 
        N =1,
        R,
        SR,
        SSR,
        UR
    }

    /// <summary>
    /// 道具名
    /// </summary>
    public string PropName;

    /// <summary>
    /// 道具编号
    /// </summary>
    public int PropID;

    /// <summary>
    /// 道具图标
    /// </summary>
    [HideInInspector]public Sprite PropIcon;

    /// <summary>
    /// 道具稀有度
    /// </summary>
    public Rarities Rarity;

    /// <summary>
    /// 道具是否为可消耗型
    /// </summary>
    public bool Consumable;

    /// <summary>
    /// 道具功能介绍
    /// </summary>
    [TextArea]public string PropDesc;

    /// <summary>
    /// 道具获取方法
    /// </summary>
    [TextArea]public string WaytoGet;

    /// <summary>
    /// 其他说明，吐槽式的解释
    /// </summary>
    [TextArea]public string OtherDesc;

    /// <summary>
    /// 道具功能函数
    /// </summary>
    public PropFunc PropFunc;
}
