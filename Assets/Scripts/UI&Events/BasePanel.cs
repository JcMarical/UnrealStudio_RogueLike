using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class BasePanel
    {
        public UIType uiType;

        public GameObject ActiveObj;

        public BasePanel(UIType uitype)
        {
            uiType = uitype;
        }

        public virtual void OnStart()
        {
            Debug.Log($"{uiType.Name}!");
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
}
