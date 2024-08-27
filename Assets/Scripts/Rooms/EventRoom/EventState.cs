using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventState : MonoBehaviour
{
    protected EventRoomMgr mgr;

    public EventState(EventRoomMgr mgr)
    {
        this.mgr = mgr;
    }

    public abstract void OnEnter();
    public abstract void LogicUpdate();
    public abstract void OnExit();
}

/// <summary>
/// 无辜的羔羊
/// </summary>
public class InnocentLambState : EventState
{
    List<GameObject> sheepList;

    public InnocentLambState(EventRoomMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        GameObject s1 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[0, 0], Quaternion.identity);
        GameObject s2 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[13, 0], Quaternion.identity);
        GameObject s3 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[0, 7], Quaternion.identity);
        GameObject s4 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[13, 7], Quaternion.identity);
        sheepList.Add(s1);
        sheepList.Add(s2);
        sheepList.Add(s3);
        sheepList.Add(s4);
        s1.GetComponent<Sheep>().sheepList = sheepList;
        s2.GetComponent<Sheep>().sheepList = sheepList;
        s3.GetComponent<Sheep>().sheepList = sheepList;
        s4.GetComponent<Sheep>().sheepList = sheepList;
    }

    public override void LogicUpdate()
    {
        if (sheepList.Count <= 0)
            mgr.ExitState();
    }

    public override void OnExit()
    {
        //TODO: 掉落随机稀有度为2的物品
    }
}

/// <summary>
/// 铜牌打手
/// </summary>
public class BronzeMedalStrikerState : EventState
{
    public BronzeMedalStrikerState(EventRoomMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
