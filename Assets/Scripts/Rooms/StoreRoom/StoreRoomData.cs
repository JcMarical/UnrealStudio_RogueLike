using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreRoomData",menuName = "Data/Rooms/StoreRoom",order =0)]
public class StoreRoomData : ScriptableObject
{
    public int MoneyStoreMaximums;

    public int GoodsAmount = 3;
    
    public bool ifSpecialGoods = false;
}
