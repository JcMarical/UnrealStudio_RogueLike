using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PropFunc CurrentFunc;
    public PropData CurrentProp;
    [SerializeField] public List<PropData> PropsinHand;
    [SerializeField] public List<PropFunc> FuncinHand;
    void Start()
    {
        CurrentFunc = FuncinHand[0];
        CurrentProp = PropsinHand[0];
        CurrentFunc.UseProp();
    }

    void Update()
    {

    }
}
