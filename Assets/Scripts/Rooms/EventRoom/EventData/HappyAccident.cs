using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 意外之喜
/// </summary>
[CreateAssetMenu(fileName = "HappyAccident", menuName = "Data/Events/HappyAccident", order = 6)]
public class HappyAccident : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        Player.Instance.realLucky++;
        //TODO: 给与玩家任意1级物品/武器
    }
}
