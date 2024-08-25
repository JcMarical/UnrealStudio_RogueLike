using MainPlayer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Rarities// 道具稀有度，从N到UR稀有度依次递增，N对应1，以此类推
{
    Resource = 0,
    N = 1,
    R,
    SR,
    SSR,
    UR
}

public class ObtainableObjectData : ScriptableObject , ITradable
{
    public string Name;// 物品名

    public int ID;// 编号

    public Sprite Icon;// 图标

    public Rarities Rarity;// 稀有度

    [TextArea] public string PropDesc;// 功能介绍

    [TextArea] public string HowtoGet;// 获取方法

    [TextArea] public string OtherDesc;// 其他说明，吐槽式的解释

    public PropFunc PropFunc;// 功能函数

    [Header("拾取时动画效果")]
    public GameObject InstancePrefab;
    public AnimationCurve curve;
    public float Height =5;
    public float Duration =0.8f;

    public IEnumerator OnDistributed(Vector3 start,GameObject target)
    {
        float localscale=0;
        GameObject theObject = Instantiate(InstancePrefab);
        if (theObject.GetComponent<SpriteRenderer>())
        { 
            theObject.GetComponent<SpriteRenderer>().sprite = Icon;
            localscale = theObject.transform.localScale.x;
        }
        
        float timer = 0;

        while (timer < Duration)
        { 
            timer += Time.deltaTime;

            var timrpercent = timer / Duration;
            var heightpercent = curve.Evaluate(timrpercent);
            var height = Mathf.Lerp(0, Height, heightpercent);

            theObject.transform.position = Vector3.Lerp(start, target.transform.position, timrpercent) + Vector3.up * height;
            theObject.transform.localScale = curve.Evaluate(timrpercent) * Vector3.one * localscale;
            yield return null;
        }
        yield return null;
    }

    public virtual void BeBought(Vector3 startPos)
    {
        StoreRoomMgr.Instance.StartCoroutine(OnDistributed(startPos, GameObject.FindGameObjectWithTag("Player")));
    }

    public void BeSoldOut()
    {
    }


    public int Price { get; set; }

    private GoodType _goodtype = GoodType.ObtainableObject;

    public GoodType GoodType
    {
        get => _goodtype; set => _goodtype = value;
    } 
}

   

