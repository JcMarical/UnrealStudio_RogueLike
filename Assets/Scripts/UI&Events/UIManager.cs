using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : TInstance<UIManager>
{
    public delegate void ChangeProperty(Property property, float changedValue);
    public event ChangeProperty ChangePropertyEvent;

    public BasePanel startpanel;
    [SerializeField] BasePanel[] panelTable; 
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
        foreach (BasePanel panel in panelTable)
        {
            panel.Initialization();
        }
        
    }
    private void Start()
    {
        push(startpanel);
    }
    //TODO:��BasePanel�л�ȡ�򿪣��رս���ʱ��Ҫִ�еķ���
    public void push(BasePanel nowPanel)
    {
        if(uistack.Count > 0) 
            uistack.Peek().OnDisable();

        uistack.Push(nowPanel);
        uistack.Peek().OnEnable();
    }
    public void pop() 
    {
        if (uistack.Count > 0)
        {
            uistack.Pop().OnDisable();
            uistack.Peek().OnEnable();
        }
    }
}

