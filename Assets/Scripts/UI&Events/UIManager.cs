using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class UIManager : TInstance<UIManager>
    {
        /// <summary>
        /// panel字典,表示名称字段和游戏内panel物体对应关系
        /// </summary>
        public Dictionary<string, GameObject> dict_uiObject;
        /// <summary>
        /// 存储UI Panel的栈
        /// </summary>
        public Stack<BasePanel> uistack;
        /// <summary>
        /// 当前场景Canvas
        /// </summary>
        public GameObject CanvasObj;

        protected override void Awake()
        {
            base.Awake();
        }
        public GameObject GetSingleObject(UIType uIType)
        {
            if (!CanvasObj)
            {
                Debug.LogError("未加入Canvas!");
                return null;
            }  
            if(dict_uiObject.ContainsKey(uIType.Name))
            {
                return dict_uiObject[uIType.Name];
            }
            
            
            GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uIType.Path),CanvasObj.transform);
            return gameObject;
        }
        public void Push(BasePanel basePanel_push)
        {
            Debug.Log($"{basePanel_push.uiType.Name}入栈");
            //获取对应的panel物体
            GameObject pushObj = GetSingleObject(basePanel_push.uiType);
            dict_uiObject.Add(basePanel_push.uiType.Name,pushObj);
            basePanel_push.ActiveObj = pushObj;

            if(uistack.Count == 0)
            {
                uistack.Push(basePanel_push);
            }
            else
            {
                if(uistack.Peek().uiType.Name != basePanel_push.uiType.Name)
                {
                    uistack.Peek().OnDisable();
                    uistack.Push(basePanel_push);
                }
            }

            basePanel_push.OnStart();
        }
    /// <summary>
    /// isload表示是否在切换场景，切换/加载场景时需要清空栈
    /// </summary>
    /// <param name="isload"></param>
        public void Pop(bool isload)
        {
            if (uistack.Count > 0)
            {
                uistack.Peek().OnDisable();
                uistack.Peek().OnDestory();
                GameObject.Destroy(dict_uiObject[uistack.Peek().uiType.Name]);
                dict_uiObject.Remove(uistack.Peek().uiType.Name);
                uistack.Pop();
                //需要清空则递归调用
                if(isload)  
                    Pop(true);
                else
                {
                    if(uistack.Count > 0)
                        uistack.Peek().OnEnable();
                }
            }
        }
    }
}
