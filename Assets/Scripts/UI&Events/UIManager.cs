using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{

    public BasePanel startpanel;
    public BasePanel[] panelTable; 

    Dictionary<System.Type,BasePanel>panelDictionary;
    /// <summary>
    /// 存储UI Panel的栈
    /// </summary>
    public Stack<BasePanel> uistack = new Stack<BasePanel>();
    /// <summary>
    /// 当前场景Canvas
    /// </summary>
    public GameObject CanvasObj;

    protected void OnEnable()
    {
        panelDictionary = new Dictionary<System.Type, BasePanel>(panelTable.Length);
        foreach (BasePanel panel in panelTable)
        {
            panel.Initialization();
            panelDictionary.Add(panel.GetType(), panel);

        }
        push(startpanel);
    }
    //TODO:从BasePanel中获取打开，关闭界面时需要执行的方法

    public void push(System.Type type)
    {
        push(panelDictionary[type]);
    }
    public void push(BasePanel nowPanel)
    {
        if (uistack.Count > 0)
        {
            uistack.Peek().gameObject.SetActive(false);
        }

        uistack.Push(nowPanel);
        uistack.Peek().gameObject.SetActive(true);
    }

    public void pop() 
    {
        if (uistack.Count > 0)
        {
            uistack.Pop().gameObject.SetActive(false);
            uistack.Peek().gameObject.SetActive(true);
        }
    }
}

