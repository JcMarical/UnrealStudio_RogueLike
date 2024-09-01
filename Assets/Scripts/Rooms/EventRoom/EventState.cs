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
        sheepList = new List<GameObject>();

        GameObject sheep1 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[0, 0], Quaternion.identity);
        GameObject sheep2 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[13, 0], Quaternion.identity);
        GameObject sheep3 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[0, 7], Quaternion.identity);
        GameObject sheep4 = Instantiate(mgr.currentEvent.enemys[0], mgr.currentRoom.positions[13, 7], Quaternion.identity);
        sheepList.Add(sheep1);
        sheepList.Add(sheep2);
        sheepList.Add(sheep3);
        sheepList.Add(sheep4);
        sheep1.GetComponent<Sheep>().enemyList = sheepList;
        sheep2.GetComponent<Sheep>().enemyList = sheepList;
        sheep3.GetComponent<Sheep>().enemyList = sheepList;
        sheep4.GetComponent<Sheep>().enemyList = sheepList;
    }

    public override void LogicUpdate()
    {
        if (sheepList.Count <= 0)
            mgr.ExitState();
    }

    public override void OnExit()
    {
        float rng = Random.Range(0, 100);
        if (rng < 50)
            mgr.DropCollection(2, true);
        else
            mgr.DropProp(2);
    }
}

/// <summary>
/// 铜牌打手
/// </summary>
public class BronzeMedalStrikerState : EventState
{
    List<GameObject> enemyList;

    public BronzeMedalStrikerState(EventRoomMgr mgr) : base(mgr)
    {

    }

    public override void OnEnter()
    {
        enemyList = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            GameObject enemy = Instantiate(mgr.currentEvent.enemys[Random.Range(0, mgr.currentEvent.enemys.Length)], mgr.currentRoom.validPositionsList[Random.Range(0, mgr.currentRoom.validPositionsList.Count)], Quaternion.identity);
            enemyList.Add(enemy);
            enemy.GetComponent<Enemy>().enemyList = enemyList;
        }
    }

    public override void LogicUpdate()
    {
        if (enemyList.Count <= 0)
            mgr.ExitState();
    }

    public override void OnExit()
    {
        mgr.DropCollection(mgr.currentEvent.items[0], true);
    }
}
