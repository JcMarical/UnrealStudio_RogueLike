using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Test_Func",menuName ="Data/Props/Func/Test_Func",order =1)]
public class Test_Func : PropFunc
{
    public override void UseProp()
    {
        Debug.Log("Test_Func");
    }
}
