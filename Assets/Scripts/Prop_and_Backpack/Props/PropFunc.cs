using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropFunc : ScriptableObject
{
    [TextArea]public string FuncDesc;// 道具功能介绍
    public float AwakeTime;
    public bool isDone;//是否已经完成使用

    public virtual void OnAwake() { }

    /// <summary>
    /// 道具的功能函数
    /// </summary>
    public virtual void UseProp() { }

    public virtual void Finish() { }
}
