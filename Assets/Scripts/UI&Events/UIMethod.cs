using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class UIMethod : TInstance<UIMethod>
    {
        /// <summary>
        /// 获取场景内Canvas
        /// </summary>
        /// <returns></returns>
        public GameObject FindCanvas()
        {
            GameObject gameObj = GameObject.FindObjectOfType<Canvas>().gameObject;
            if (gameObj == null)
            {
                Debug.LogError("未找到canvas");
            }

            return gameObj;
        }

        public GameObject FindObjectInChild(GameObject panel, string child_name)
        {
            Transform[] transforms = panel.GetComponentsInChildren<Transform>();

            foreach (var t in transforms)
            {
                if (t.gameObject.name == child_name)
                    return t.gameObject;
            }
            Debug.LogError($"未找到{child_name}!");
            return null;
        }
    }
}
