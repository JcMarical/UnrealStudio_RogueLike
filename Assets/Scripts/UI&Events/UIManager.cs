using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class UIManager : TInstance<UIManager>
    {
        /// <summary>
        /// panel�ֵ�,��ʾ�����ֶκ���Ϸ��panel�����Ӧ��ϵ
        /// </summary>
        public Dictionary<string,BasePanel> dict_uiObject;
        /// <summary>
        /// �洢UI Panel��ջ
        /// </summary>
        public Stack<BasePanel> uistack;
        /// <summary>
        /// ��ǰ����Canvas
        /// </summary>
        public GameObject CanvasObj;

        protected override void Awake()
        {
            base.Awake();
        }
        //TODO:��BasePanel�л�ȡ�򿪣��رս���ʱ��Ҫִ�еķ���
    }
}
