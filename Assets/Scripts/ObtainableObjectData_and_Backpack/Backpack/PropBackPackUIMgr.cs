using System;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using ImageElement = UnityEngine.UIElements.Image;
using DG.Tweening;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;

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
    [SerializeField]private GameObject PropUIContainer;//�������UI
    [SerializeField]private List<Prop_Data> Props;//�������

    [Header("��Դ��Ϣ")]
    public Resource_Data StoredCoins;//�������
    public Resource_Data StoredDices;//��������


    private int _currenetCoins;
    [OdinSerialize]public int CurrenetCoins
    {
        get => _currenetCoins;
        set
        {
            if (value != _currenetCoins)
            { 
                _currenetCoins = value;
                WhenCoinChanged?.Invoke(_currenetCoins);
            }
        }
    }
    public bool ConsumeCoin(int Amount)
    {
        if (CurrenetCoins >= Amount)
        { 
            CurrenetCoins -= Amount;
            for (int i=0;i < Amount ;i++ )
                WhenCoinBeUesed?.Invoke();
            return true;
        }
        return false;
    }

    public void GainCoin(int Amount)
    {
        CurrenetCoins += Amount;
    }


    private int _currenetDices;
    [OdinSerialize]public int CurrenetDices
    {
        get => _currenetDices;
        set
        {
            if (value != _currenetDices)
            { 
                _currenetDices = value;
                WhenDiceChanged?.Invoke(_currenetDices);
            }
        }
    }

    public bool ConsumeDice(int Amount)
    {
        if (CurrenetDices >= Amount)
        {
            CurrenetDices -= Amount;
            for (int i = 0; i < Amount; i++)
                WhenDiceBeUesed?.Invoke();
            return true;
        }

        return false;
    }

    public void GainDice(int Amount)
    { 
        CurrenetDices += Amount;
    }

    public static event Action CollecttionUpdated;
    public static event Action PropUpdated;
    public static event Action ShowPropBack;
    public static event Action HidePropBack;
    public event Action WhenDiceBeUesed;
    public event Action WhenCoinBeUesed;
    public event Action<int> WhenDiceChanged;
    public event Action<int> WhenCoinChanged;


    override protected void Awake()
    {
        base.Awake();
        //CollectionDatas.Clear();
        InitComponent();
        InitUI();

        CollecttionUpdated += UpdatePBUI;
        PropUpdated += UpdatePropsUI;
        ShowPropBack += ShowPropBackpack;
        HidePropBack += HidePropBackpack;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseProp();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            SwitchPropsList();
        }
    }

    void InitComponent()
    {
        PBUIBackGround = transform.GetChild(0).transform.GetChild(0).gameObject;
        PBUIfather = PBUIBackGround;
        PropUIContainer = transform.GetChild(0).transform.GetChild(1).gameObject;
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
    /// ���±���UI��ʾ������
    /// </summary>
    void UpdatePBUI()
    {
        int count=0;
        foreach (Collection_Data PD in CollectionDatas)
        {
            if (PD)
            {
                SetPBUI(ref PBUIContainer[count%width,count/width], PD);
                count++;
            }
        }
        for(int i=count;i < (width*height); i++)
        {
            SetPBUI(ref PBUIContainer[i % width, i / width], null);
        }
    }

    /// <summary>
    /// ���ñ�������UI��ʾ������
    /// </summary>
    /// <param name="target">Ŀ��UI����</param>
    /// <param name="aim">Ҫ���õ�����</param>
    public void SetPBUI(ref PropBackpackUI target, Collection_Data aim)
    {
        if (aim)
        {
            target.Image.sprite = aim.Icon;
            target.PropData = aim;
        }
        else
        {
            target.Image.sprite = Collection_Data.NULLData.Icon;
            target.PropData = null;
        }
    }

    /// <summary>
    /// �����²�Ʒ,�����±���UI����
    /// </summary>
    /// <param name="newProp">��UI����</param>
    public bool AddCollection(Collection_Data newCollection)
    {  
        if (CollectionDatas.Count < height * width)
        {
            CollectionDatas.Add(newCollection);
            CollecttionUpdated?.Invoke();
            return true;
        }
        else
        {
            Debug.LogError("����������������ʾUI");
            return false;
        } 
    }

    /// <summary>
    /// ����µĵ���
    /// </summary>
    /// <param name="PropData"></param>
    public bool GetProp(Prop_Data PropData)
    {
        if (PropData !=null && !PropData.IsUnityNull() )
        {
            if (Props.Count != 3)
            {
                Props.Add(PropData);
                PropUpdated?.Invoke();
                return true;
            }
            else
            {
                //TODO��������������������ʾUI
                Debug.Log("��������");
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// �����������UI��ʾ������
    /// </summary>
    void UpdatePropsUI()
    {
        int count = 0;
        for (int i=0; i<3;i++)
        {
            PropUIContainer.transform.GetChild(i).GetComponent<Image>().sprite = null;
        }
        foreach (Prop_Data prop in Props)
        { 
            PropUIContainer.transform.GetChild(count++).GetComponent<Image>().sprite = prop.Icon;
        }
    }

    /// <summary>
    /// ʹ�õ���(Ĭ��ʹ��һ��λ����)
    /// </summary>
    public void UseProp()
    {
        if (Props.Count > 0)
        {
            Props[0].PropFunc.UseProp();
            Props.RemoveAt(0);
            UpdatePropsUI();
        }
        else
        {
            Debug.Log("��ǰ�޵��߿���");
            return;
        }
    }

    /// <summary>
    /// ��ʱ���л�������(2 -> 1,3 -> 2,1 -> 3)
    /// </summary>
    public void SwitchPropsList()
    {
        if (Props.Count > 0)
        {
            Props.Add(Props[0]);
            Props.RemoveAt(0);
            PropUpdated?.Invoke();
        }
    }

    public void ReMoveProp(Prop_Data target)
    {
        Props.Remove(target);
        PropUpdated?.Invoke();
    }

    public void ReMoveCollection(Collection_Data target)
    {
        var list = new List<Collection_Data>();
        foreach (Collection_Data collection in CollectionDatas)
        {
            if (collection.ID == target.ID)
            {
                list.Add(collection);
            }
        }

        Debug.Log(list.Count);

        foreach (var col in list)
        {
            CollectionDatas.Remove(col);
        }

        CollecttionUpdated?.Invoke();
    }
}


