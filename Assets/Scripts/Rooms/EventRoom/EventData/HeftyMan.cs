using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 嘿！大块头！
/// </summary>
[CreateAssetMenu(fileName = "HeftyMan", menuName = "Data/Events/HeftyMan", order = 3)]
public class HeftyMan : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        //TODO: 给与玩家 “大块头” 藏品
    }

    public override void Choose1()
    {
        base.Choose1();
    }
}
