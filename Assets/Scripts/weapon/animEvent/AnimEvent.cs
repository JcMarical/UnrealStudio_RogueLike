using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : StateMachineBehaviour
{
    public static UnityEvent AttackEnd=new UnityEvent();
    public static UnityEvent AttackStart=new UnityEvent();
}
