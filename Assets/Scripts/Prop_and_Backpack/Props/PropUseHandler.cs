using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropUseHandler : TInstance<PropUseHandler>
{
    public static PropData NextProp_to_Use;

    override protected void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if ((bool)NextProp_to_Use?.PropFunc.isDone)
        {
            NextProp_to_Use.PropFunc.UseProp();    
        }
    }
}
