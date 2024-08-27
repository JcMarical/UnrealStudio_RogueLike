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
        public Dictionary<string,BasePanel> dict_uiObject;
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
        //TODO:从BasePanel中获取打开，关闭界面时需要执行的方法
    }
}
