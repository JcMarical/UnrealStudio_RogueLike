using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasePanel : ScriptableObject
{
    public Canvas canvas;
    public GameObject NowPanel;

    public virtual void OnStart()
    {
            
    }
    public virtual void OnEnable()
        {

        }
    public virtual void OnDisable()
        {

        }
    public virtual void OnDestory()
        {

        }
        
}
