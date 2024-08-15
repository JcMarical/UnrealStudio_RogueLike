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
    public Collection_Data PropData;
}

public class PropBackPackUIMgr : TInstance<PropBackPackUIMgr>
{
    [Header("��Ʒ����")]
    public static int width = 5, height = 5;
    public GameObject PBUIBackGround;//����UI������
    public GameObject PBUIfather;//���б���Ԫ��UI������.���µ�������Ӧ�ð�������˳������
    public PropBackpackUI[,] PBUIContainer = new PropBackpackUI[width,height];//���߱���UI��ʾ������
    public List<Collection_Data> CollectionDatas = new List<Collection_Data>();//����������������
    public Coroutine UIMoving;
    public bool UIShowing = false;//����UI�Ƿ�������ʾ

    [Header("������Ϣ")]
    public GameObject PropUI;//�������UI
    public Prop_Data Prop;//�������

    [Header("��Դ��Ϣ")]
    public Resource_Data Coins;//�������
    public Resource_Data Dices;//��������

    public static event Action PropUpdated;
    public static event Action ShowPropBack;
    public static event Action HidePropBack;

    //private Dictionary<float,Coroutine> Cortines = new Dictionary<float, Coroutine>();

    override protected void Awake()
    {
        base.Awake();
        CollectionDatas.Clear();
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
    public PropBackpackUI NewPBUI(GameObject UI,Image Image, Collection_Data PropData)
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
            UIMoving = StartCoroutine(showPropBackpack());
        }
    }

    IEnumerator showPropBackpack()
    {
        //Cortines.Add(DateTime.Now.GetHashCode(), UIMoving);
        UpdatePBUI();
        if (!UIShowing)
        {
            PBUIfather.SetActive(true);
            RectTransform RectTransform = PBUIfather.GetComponent<RectTransform>();
            if (RectTransform)
            {
                float x = RectTransform.position.x;
                float size_x = RectTransform.rect.width;
                float target_x = x - size_x;

                RectTransform.DOMoveX(target_x, 0.5f).OnComplete(() => { UIMoving = null; UIShowing = true; });
            }
        }
        else 
        {
            yield return null;
            UIMoving = null;
        }
        yield return null;
    }

    /// <summary>
    /// �������б���UI
    /// </summary>
    public void HidePropBackpack()
    {
        if (UIMoving == null)
        {
            UIMoving = StartCoroutine(hidePropBackpack());
        }
    }

    IEnumerator hidePropBackpack()
    {
        if (UIShowing)
        {
            RectTransform RectTransform = PBUIfather.GetComponent<RectTransform>();
            if (RectTransform)
            {
                float x = RectTransform.position.x;
                float size_x = RectTransform.rect.width;
                float target_x = x + size_x;

                RectTransform.DOMoveX(target_x, 0.5f).OnComplete(() => { UIMoving = null; });
            }
            UIShowing = false;
        }
        else
        {
            yield return null;
            UIMoving = null;
        }
        yield return null;
    }

    /// <summary>
    /// ����UI��ʾ������
    /// </summary>
    void UpdatePBUI()
    {
        int count=0;
        foreach (Collection_Data PD in CollectionDatas)
        {
            if (PD)
            {
                SetPBUI(PBUIContainer[count%width,count/width], PD);
                count++;
            }
        }
        for(int i=count;i < (width*height); i++)
        {
            SetPBUI(PBUIContainer[i % width, i / width], Collection_Data.NULLData);
        }
    }

    /// <summary>
    /// ���ñ���UI��ʾ������
    /// </summary>
    /// <param name="target">Ŀ��UI����</param>
    /// <param name="aim">Ҫ���õ�����</param>
    public void SetPBUI(PropBackpackUI target, Collection_Data aim)
    { 
        target.Image.sprite = aim.Icon;
        target.PropData = aim;
    }

    /// <summary>
    /// ����²�Ʒ,�����±���UI����
    /// </summary>
    /// <param name="newProp">��UI����</param>
    public void AddProp(Collection_Data newProp)
    {  
        if (CollectionDatas.Count < height * width)
        {
            CollectionDatas.Add(newProp);
            PropUpdated?.Invoke();
            Debug.Log("������������"+ CollectionDatas.Count);
        }
        else
        {
            Debug.LogError("����������������ʾUI");
        } 
    }

    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="PropData">�µ�������ߵ�����</param>
    public void SetProps(Prop_Data PropData)
    {
        if (!Prop)
        {
            Prop = PropData;
            UpdatePropsUI(); 
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
    void UpdatePropsUI()
    {
        if (Prop)
        {
            Image image = PropUI.GetComponent<Image>();
            image.sprite = Prop.Icon;  
        }
        else
        { 
            Debug.LogError("�������Ϊ��");
        }
    }

    /// <summary>
    /// ʹ�õ���
    /// </summary>
    public void UseProp()
    {
        if (Prop)
        {
            Prop.PropFunc.UseProp();
            Prop = null;
            UpdatePropsUI();
        }
        else
        {
            Debug.Log("��ǰ�޵��߿���");
            return;
        }
    }
}


