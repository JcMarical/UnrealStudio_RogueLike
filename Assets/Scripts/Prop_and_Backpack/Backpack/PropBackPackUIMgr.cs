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
    public GameObject PBUIBackGround;//背包UI背景板
    public GameObject PBUIfather;//所有背包元素UI父物体.其下的子物体应该按照行列顺序排列
    public PropBackpackUI[,] PBUIContainer = new PropBackpackUI[width,height];//道具背包UI显示层容器
    public bool UIShowing = false;//背包UI是否正在显示
    public List<PropData> PropDatas = new List<PropData>();//背包道具数据容器
    public GameObject Strategic_propsUI;//特殊道具UI
    public PropData Strategic_props;//特殊道具
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
    /// 初始化背包UI
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
    /// 新建背包UI元素
    /// </summary>
    /// <param name="UI">该UI的GameObject</param>
    /// <param name="Image">道具图像</param>
    /// <param name="PropData">道具数据</param>
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
    /// 展示所有背包UI
    /// </summary>
    public void ShowPropBackpack()
    {
        if (UIMoving == null)
        {
            Debug.Log("ShowPropBackpack:没有正在进行的背包UI移动进程");
            UIMoving = StartCoroutine(showPropBackpack());
        }
        else Debug.Log("ShowPropBackpack:有正在进行的背包UI移动进程");
    }

    IEnumerator showPropBackpack()
    {
        //Cortines.Add(DateTime.Now.GetHashCode(), UIMoving);
        //UpdatePBUI();
        if (!UIShowing)
        {
            Debug.Log("showPropBackpack：UIShowing为假");
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
        Debug.Log("协程执行完毕");
        Debug.Log("UIMoving为空"+ (UIMoving == null? true: false).ToString());
        yield return null;
    }

    /// <summary>
    /// 隐藏所有背包UI
    /// </summary>
    public void HidePropBackpack()
    {
        if (UIMoving == null)
        {
            Debug.Log("HidePropBackpack:没有正在进行的背包UI移动进程");
            StartCoroutine(hidePropBackpack());
        }
        else { Debug.Log("HidePropBackpack:有正在进行的背包UI移动进程"); }
    }

    IEnumerator hidePropBackpack()
    {
        if (UIShowing)
        {
            Debug.Log("hidePropBackpack：UIShowing为真");
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
        Debug.Log("UIMoving为空" + (UIMoving == null ? true : false).ToString());
        yield return null;
    }

    /// <summary>
    /// 更新UI显示层数据
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
    /// 设置背包UI显示层数据
    /// </summary>
    /// <param name="target">目标UI物体</param>
    /// <param name="aim">要设置的数据</param>
    public void SetPBUI(PropBackpackUI target,PropData aim)
    { 
        target.Image.sprite = aim.PropIcon;
        target.PropData = aim;
    }

    /// <summary>
    /// 添加新UI,并更新UI数据层
    /// </summary>
    /// <param name="newProp">新UI数据</param>
    public void AddProp(PropData newProp)
    {
        if (newProp)
        {
            if (PropDatas.Count < height * width)
            {
                PropDatas.Add(newProp);
                PropUpdated?.Invoke();
                Debug.Log("现有数据数量"+ PropDatas.Count);
            }
            else
            {
                Debug.LogError("背包已满，处理提示UI");
            } 
        }
    }

    /// <summary>
    /// 更新特殊道具数据
    /// </summary>
    /// <param name="PropData">新的特殊道具的数据</param>
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
                Debug.LogError("该道具不是特殊道具");
            } 
        }
        else
        {
            Debug.Log("特殊道具已存在");
            //TODO:处理特殊道具已存在的情况
        }
    }

    /// <summary>
    /// 更新特殊道具UI显示层数据
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
            Debug.LogError("特殊道具为空");
        }
    }
}


