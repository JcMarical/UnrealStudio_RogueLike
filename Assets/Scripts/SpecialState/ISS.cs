using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISS
{
    //玩家特殊状态
    public void SS_Burn()
    {
        Debug.Log("SS_Burn()默认实现");
        return;
    }

    //敌人特殊状态
    public void SS_Burn(float a)
    {
        Debug.Log("SS_Burn(float a)默认实现");
        return;
    }
}

