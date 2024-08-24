using MainPlayer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Rarities// ����ϡ�жȣ���N��URϡ�ж����ε�����N��Ӧ1���Դ�����
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
    public string Name;// ��Ʒ��

    public int ID;// ���

    public Sprite Icon;// ͼ��

    public Rarities Rarity;// ϡ�ж�

    [TextArea] public string PropDesc;// ���ܽ���

    [TextArea] public string HowtoGet;// ��ȡ����

    [TextArea] public string OtherDesc;// ����˵�����²�ʽ�Ľ���

    public PropFunc PropFunc;// ���ܺ���

    [Header("ʰȡʱ����Ч��")]
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

   

