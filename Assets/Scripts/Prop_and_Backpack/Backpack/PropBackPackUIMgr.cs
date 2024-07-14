using System;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using ImageElement = UnityEngine.UIElements.Image;
using DG.Tweening;
using System.Collections;

public struct PropBackpackUI
{
    public GameObject UI;
    public Image Image;
    public PropData PropData;
}

public class PropBackPackUIMgr : TInstance<PropBackPackUIMgr>
{
    public static int width = 5, height = 5;
    public GameObject PBUIBackGround;//����UI������
    public GameObject PBUIfather;//���б���Ԫ��UI������.���µ�������Ӧ�ð�������˳������
    public PropBackpackUI[,] PBUIContainer = new PropBackpackUI[width,height];//���߱���UI��ʾ������
    public bool UIShowing = false;//����UI�Ƿ�������ʾ
    public List<PropData> PropDatas = new List<PropData>();//����������������
    public GameObject Strategic_propsUI;//�������UI
    public PropData Strategic_props;//�������
    public Coroutine UIMoving;

    public static event Action PropUpdated;
    public static event Action ShowPropBack;
    public static event Action HidePropBack;

    //private Dictionary<float,Coroutine> Cortines = new Dictionary<float, Coroutine>();

    override protected void Awake()
    {
        base.Awake();
        PropDatas.Clear();
        InitUI();

        PropUpdated += UpdatePBUI;
        ShowPropBack += ShowPropBackpack;
        HidePropBack += HidePropBackpack;

    }

    /// <summary>
    /// ��ʼ������UI
    /// </summary>
    void InitUI()
    {
        GameObject newUI;
        Image newImage;
        for (int i = 0; i < width * height; i++)
        {
            newUI = PBUIfather.transform.GetChild(i).gameObject;
            newImage = newUI.GetComponent<Image>();
            PBUIContainer[i % width, i / width] = NewPBUI(newUI, newImage, null);
        }
    }

    /// <summary>
    /// �½�����UIԪ��
    /// </summary>
    /// <param name="UI">��UI��GameObject</param>
    /// <param name="Image">����ͼ��</param>
    /// <param name="PropData">��������</param>
    /// <returns></returns>
    public PropBackpackUI NewPBUI(GameObject UI,Image Image,PropData PropData)
    { 
        PropBackpackUI newPBUI = new PropBackpackUI();
        newPBUI.Image = Image;
        newPBUI.UI = UI;
        newPBUI.PropData = PropData;
        return newPBUI;
    }

    /// <summary>
    /// չʾ���б���UI
    /// </summary>
    public void ShowPropBackpack()
    {
        if (UIMoving == null)
        {
            Debug.Log("ShowPropBackpack:û�����ڽ��еı���UI�ƶ�����");
            UIMoving = StartCoroutine(showPropBackpack());
        }
        else Debug.Log("ShowPropBackpack:�����ڽ��еı���UI�ƶ�����");
    }

    IEnumerator showPropBackpack()
    {
        //Cortines.Add(DateTime.Now.GetHashCode(), UIMoving);
        //UpdatePBUI();
        if (!UIShowing)
        {
            Debug.Log("showPropBackpack��UIShowingΪ��");
            PBUIfather.SetActive(true);
            RectTransform RectTransform = PBUIfather.GetComponent<RectTransform>();
            if (RectTransform)
            {
                float x = RectTransform.transform.position.x;
                float size_x = RectTransform.rect.width;
                float target_x = x - size_x;

                RectTransform.DOMoveX(target_x, 0.5f).OnComplete(() => { UIMoving = null; UIShowing = true; });
            }
        }
        else 
        {
            UIMoving = null;
        }
        Debug.Log("Э��ִ�����");
        Debug.Log("UIMovingΪ��"+ (UIMoving == null? true: false).ToString());
        yield return null;
    }

    /// <summary>
    /// �������б���UI
    /// </summary>
    public void HidePropBackpack()
    {
        if (UIMoving == null)
        {
            Debug.Log("HidePropBackpack:û�����ڽ��еı���UI�ƶ�����");
            StartCoroutine(hidePropBackpack());
        }
        else { Debug.Log("HidePropBackpack:�����ڽ��еı���UI�ƶ�����"); }
    }

    IEnumerator hidePropBackpack()
    {
        if (UIShowing)
        {
            Debug.Log("hidePropBackpack��UIShowingΪ��");
            RectTransform RectTransform = PBUIfather.GetComponent<RectTransform>();
            if (RectTransform)
            {
                float x = RectTransform.transform.position.x;
                float size_x = RectTransform.rect.width;
                float target_x = x + size_x;

                RectTransform.DOMoveX(target_x, 0.5f).OnComplete(() => { PBUIfather.SetActive(false); UIMoving = null; });
            }
            UIShowing = false;
        }
        else UIMoving = null;
        Debug.Log("UIMovingΪ��" + (UIMoving == null ? true : false).ToString());
        yield return null;
    }

    /// <summary>
    /// ����UI��ʾ������
    /// </summary>
    void UpdatePBUI()
    {
        int count=0;
        foreach (PropData PD in PropDatas)
        {
            if (PD)
            {
                SetPBUI(PBUIContainer[count%width,count/width], PD);
                count++;
            }
        }
        for(int i=count;i < (width*height); i++)
        {
            SetPBUI(PBUIContainer[i % width, i / width],PropData.NULLData);
        }
    }

    /// <summary>
    /// ���ñ���UI��ʾ������
    /// </summary>
    /// <param name="target">Ŀ��UI����</param>
    /// <param name="aim">Ҫ���õ�����</param>
    public void SetPBUI(PropBackpackUI target,PropData aim)
    { 
        target.Image.sprite = aim.PropIcon;
        target.PropData = aim;
    }

    /// <summary>
    /// �����UI,������UI���ݲ�
    /// </summary>
    /// <param name="newProp">��UI����</param>
    public void AddProp(PropData newProp)
    {
        if (newProp)
        {
            if (PropDatas.Count < height * width)
            {
                PropDatas.Add(newProp);
                PropUpdated?.Invoke();
                Debug.Log("������������"+ PropDatas.Count);
            }
            else
            {
                Debug.LogError("����������������ʾUI");
            } 
        }
    }

    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="PropData">�µ�������ߵ�����</param>
    public void SetStrategic_props(PropData PropData)
    {
        if (!Strategic_props)
        { 
            if (PropData.Consumable)
            {
                Strategic_props = PropData;
                UpdateStrategic_propsUI();
            }
            else
            {
                Debug.LogError("�õ��߲����������");
            } 
        }
        else
        {
            Debug.Log("��������Ѵ���");
            //TODO:������������Ѵ��ڵ����
        }
    }

    /// <summary>
    /// �����������UI��ʾ������
    /// </summary>
    void UpdateStrategic_propsUI()
    {
        if (Strategic_props)
        {
            Image image = Strategic_propsUI.GetComponent<Image>();
            image.sprite = Strategic_props.PropIcon;  
        }
        else
        { 
            Debug.LogError("�������Ϊ��");
        }
    }
}


