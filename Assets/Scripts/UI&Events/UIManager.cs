using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : TInstance<UIManager>
{
    public BasePanel startpanel;
    [SerializeField] BasePanel[] panelTable; 
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
        foreach (BasePanel panel in panelTable)
        {
            panel.Initialization();
        }
        
    }
    private void Start()
    {
        push(startpanel);
    }
    //TODO:从BasePanel中获取打开，关闭界面时需要执行的方法
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

