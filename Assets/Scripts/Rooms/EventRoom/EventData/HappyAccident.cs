using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 意外之喜
/// </summary>
[CreateAssetMenu(fileName = "HappyAccident", menuName = "Data/Events/HappyAccident", order = 6)]
public class Event_HappyAccident : EventData
{
    public override void Choose0()
    {
        base.Choose0();

        Player.Instance.realLucky++;

        float rng = Random.Range(0, 150);
        if (rng < 50)
            EventRoomMgr.Instance.DropCollection(1, false);
        else if (rng < 100)
            EventRoomMgr.Instance.DropProp(1);
        else
        {
            //TODO: 掉落随机1级武器
        }
    }
}
