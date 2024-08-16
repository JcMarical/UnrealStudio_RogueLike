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
    [Header("藏品背包")]
    public static int width = 5, height = 5;
    public GameObject PBUIBackGround;//背包UI背景板
    public GameObject PBUIfather;//所有背包元素UI父物体.其下的子物体应该按照行列顺序排列
    public PropBackpackUI[,] PBUIContainer = new PropBackpackUI[width,height];//道具背包UI显示层容器
    public List<Collection_Data> CollectionDatas = new List<Collection_Data>();//背包道具数据容器
    public Coroutine UIMoving;
    public bool UIShowing = false;//背包UI是否正在显示

    [Header("道具信息")]
    public GameObject PropUI;//特殊道具UI
    public Prop_Data Prop;//特殊道具

    [Header("资源信息")]
    public Resource_Data Coins;//金币数量
    public Resource_Data Dices;//骰子数量

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
    public PropBackpackUI NewPBUI(GameObject UI,Image Image, Collection_Data PropData)
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
    /// 隐藏所有背包UI
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
    /// 更新UI显示层数据
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
    /// 设置背包UI显示层数据
    /// </summary>
    /// <param name="target">目标UI物体</param>
    /// <param name="aim">要设置的数据</param>
    public void SetPBUI(PropBackpackUI target, Collection_Data aim)
    { 
        target.Image.sprite = aim.Icon;
        target.PropData = aim;
    }

    /// <summary>
    /// 添加新藏品,并更新背包UI界面
    /// </summary>
    /// <param name="newProp">新UI数据</param>
    public void AddProp(Collection_Data newProp)
    {  
        if (CollectionDatas.Count < height * width)
        {
            CollectionDatas.Add(newProp);
            PropUpdated?.Invoke();
            Debug.Log("现有数据数量"+ CollectionDatas.Count);
        }
        else
        {
            Debug.LogError("背包已满，处理提示UI");
        } 
    }

    /// <summary>
    /// 更新特殊道具数据
    /// </summary>
    /// <param name="PropData">新的特殊道具的数据</param>
    public void SetProps(Prop_Data PropData)
    {
        if (!Prop)
        {
            Prop = PropData;
            UpdatePropsUI(); 
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
    void UpdatePropsUI()
    {
        if (Prop)
        {
            Image image = PropUI.GetComponent<Image>();
            image.sprite = Prop.Icon;  
        }
        else
        { 
            Debug.LogError("特殊道具为空");
        }
    }

    /// <summary>
    /// 使用道具
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
            Debug.Log("当前无道具可用");
            return;
        }
    }
}


