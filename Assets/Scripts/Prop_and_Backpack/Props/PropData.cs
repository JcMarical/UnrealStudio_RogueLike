using UnityEngine;

[CreateAssetMenu(fileName ="PropData",menuName ="Data/Props/Data",order =1)]
public class PropData : ScriptableObject
{
    public enum Rarities// 道具稀有度，从N到UR稀有度依次递增，N对应1，以此类推
    { 
        N =1,
        R,
        SR,
        SSR,
        UR
    }

    public string PropName;// 道具名

    public int PropID;// 道具编号

    [HideInInspector]public Sprite PropIcon;// 道具图标

    public Rarities Rarity;// 道具稀有度

    public bool Consumable;// 道具是否为可消耗型

    [TextArea]public string PropDesc;// 道具功能介绍

    [TextArea]public string HowtoGet;// 道具获取方法

    [TextArea]public string OtherDesc;// 其他说明，吐槽式的解释

    public PropFunc PropFunc;// 道具功能函数

    public static PropData NULLData
    {
        get 
        { 
            PropData NULLData = CreateInstance<PropData>();
            NULLData.OtherDesc = "NULL";
            NULLData.PropDesc = "NULL";
            NULLData.PropFunc = null;
            NULLData.PropIcon = null;
            NULLData.PropID = -1;
            NULLData.PropName = "NULL";
            NULLData.Rarity = Rarities.N;
            NULLData.HowtoGet = "NULL";
            NULLData.Consumable = false;

            return NULLData;
        }
    }
}
