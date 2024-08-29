using DG.Tweening;
using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Event
{
    InnocentLamb,
    BronzeMedalStriker,
}

public class EventRoomMgr : TInstance<EventRoomMgr>
{
    public EventData currentEvent;
    public List<EventData> eventList;
    public EventRoom currentRoom;

    private Sequence sequence;

    [Serializable]
    public struct Choice
    {
        public RectTransform buttonTransform;
        public Text title;
        public Text description;
    }

    private bool isPlay;
    public bool canContinue;
    public int choiceNumber;
    private int phase;

    [Header("UI")]
    [Space(16)]
    public RectTransform canvas;
    public GameObject EventRoomUI;
    public Image backgroundImage;
    public Text eventTitle;
    public RectTransform backgroundAndTitle;
    private string words;
    [HideInInspector] public string[] choiceExtraWords;
    [HideInInspector] public string resultExtraWords;
    public Text eventDescription;
    [Space(16)]
    public Choice[] choices;
    [Space(16)]
    public RectTransform closeButton;

    [Header("FSM")]
    private EventState currentState;
    private EventState innocentLambState;
    private EventState bronzeMedalStrikerState;

    protected override void Awake()
    {
        base.Awake();

        innocentLambState = new InnocentLambState(this);
        bronzeMedalStrikerState = new BronzeMedalStrikerState(this);

        choiceExtraWords = new string[4];

        closeButton.GetComponent<Button>().onClick.AddListener(CloseMenu);
    }

    private void Update()
    {
        currentState?.LogicUpdate();

        if (isPlay && Input.GetMouseButtonDown(0))
            SkipAnimation();
    }

    public void EnterState(Event state)
    {
        currentState = state switch
        {
            Event.InnocentLamb => innocentLambState,
            Event.BronzeMedalStriker => bronzeMedalStrikerState,

            _ => null
        };

        currentState.OnEnter();
    }

    public void ExitState()
    {
        currentState.OnExit();
        currentState = null;
    }

    /// <summary>
    /// 进入事件
    /// </summary>
    [ContextMenu("进入随机事件")]
    public void EnterEvent()
    {
        //禁用玩家操作
        BindingChange.Instance.inputControl.Disable();
        Player.Instance.GetComponentInChildren<PlayerAnimation>().inputControl.Disable();

        //随机抽取事件
        EventData randomEvent;

        do
        {
            randomEvent = eventList[UnityEngine.Random.Range(0, eventList.Count)];

        } while (randomEvent.layer != 0 && GameManager.Instance.CurrentLayer != randomEvent.layer);

        currentEvent = randomEvent;
        if (!randomEvent.isRepeatable)
            eventList.Remove(randomEvent);

        //初始化事件数据
        currentEvent.InitializeExtraWords();
        backgroundImage = currentEvent.backgroundImage;
        eventTitle.text = currentEvent.eventTitle;
        words = currentEvent.eventDescription;
        eventDescription.text = null;
        for (int i = 0; i < currentEvent.choices.Length; i++)
        {
            choices[i].title.text = currentEvent.choices[i].title;
            choices[i].description.text = currentEvent.choices[i].description;
            choices[i].buttonTransform.parent.gameObject.SetActive(true);
        }

        //初始化按钮委托
        canContinue = true;
        choices[0].buttonTransform.GetComponent<Button>().onClick.AddListener(currentEvent.Choose0);
        choices[1].buttonTransform.GetComponent<Button>().onClick.AddListener(currentEvent.Choose1);
        choices[2].buttonTransform.GetComponent<Button>().onClick.AddListener(currentEvent.Choose2);
        choices[3].buttonTransform.GetComponent<Button>().onClick.AddListener(currentEvent.Choose3);
        for (int i = 0; i < choices.Length; i++)
            choices[i].buttonTransform.GetComponent<Button>().onClick.AddListener(ContinueEvent);

        //初始化UI位置
        backgroundAndTitle.position = new Vector3(backgroundAndTitle.position.x, canvas.rect.height * canvas.lossyScale.y * 2.5f, 0);
        for (int i = 0; i < currentEvent.choices.Length; i++)
            choices[i].buttonTransform.position = new Vector3(canvas.rect.width * canvas.lossyScale.x * 1.2f, choices[i].buttonTransform.position.y, 0);

        EventRoomUI.SetActive(true);

        //设置UI动画
        sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.Append(backgroundAndTitle.DOMoveY(canvas.rect.height * canvas.lossyScale.y * 0.5f, 2));
        sequence.Append(eventDescription.DOText(words, words.Length * 0.05f));
        for (int i = 0; i < currentEvent.choices.Length; i++)
            sequence.Append(choices[i].buttonTransform.DOMoveX(canvas.rect.width * canvas.lossyScale.x * 0.8f, 1));
        sequence.AppendCallback(() => isPlay = false);

        sequence.Play();
        phase = 0;
        isPlay = true;
    }

    /// <summary>
    /// 做出选择后续
    /// </summary>
    public void ContinueEvent()
    {
        if (!canContinue)
            return;

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].buttonTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            choices[i].buttonTransform.parent.gameObject.SetActive(false);
        }

        words = currentEvent.choices[choiceNumber].result + resultExtraWords;
        eventDescription.text = null;
        closeButton.position = new Vector3(canvas.rect.width * canvas.lossyScale.x * 1.2f, closeButton.position.y, 0);
        closeButton.gameObject.SetActive(true);

        //设置UI动画
        sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.Append(eventDescription.DOText(words, words.Length * 0.05f));
        sequence.Append(closeButton.DOMoveX(canvas.rect.width * canvas.lossyScale.x * 0.8f, 1));
        sequence.AppendCallback(() => isPlay = false);

        sequence.Play();
        phase = 1;
        isPlay = true;
    }

    /// <summary>
    /// 跳过当前UI动画
    /// </summary>
    private void SkipAnimation()
    {
        sequence.Kill();

        switch (phase)
        {
            case 0:
                backgroundAndTitle.position = new Vector3(backgroundAndTitle.position.x, canvas.rect.height * canvas.lossyScale.y * 0.5f, 0);
                eventDescription.text = words;
                for (int i = 0; i < currentEvent.choices.Length; i++)
                    choices[i].buttonTransform.position = new Vector3(canvas.rect.width * canvas.lossyScale.x * 0.8f, choices[i].buttonTransform.position.y, 0);
                break;
            case 1:
                eventDescription.text = words;
                closeButton.position = new Vector3(canvas.rect.width * canvas.lossyScale.x * 0.8f, closeButton.position.y, 0);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 关闭事件UI
    /// </summary>
    private void CloseMenu()
    {
        closeButton.gameObject.SetActive(false);
        EventRoomUI.SetActive(false);

        BindingChange.Instance.inputControl.Enable();
        Player.Instance.GetComponentInChildren<PlayerAnimation>().inputControl.Enable();
    }

    /// <summary>
    /// 掉落指定等级的藏品
    /// </summary>
    /// <param name="level">藏品等级</param>
    /// <param name="isDirectlyAdd">是否直接添加到玩家背包</param>
    public void DropCollection(int level, bool isDirectlyAdd)
    {
        if (isDirectlyAdd)
        {
            Collection_Data item = PropDistributor.Instance.DistributeRandomCollectionbyLevel(level);
            if (item)
            {
                StartCoroutine(item.OnDistributed(currentRoom.centerPosition.position, GameObject.FindGameObjectWithTag("Player").transform.position));
                PropBackPackUIMgr.Instance.AddCollection(item);
            }
        }
        else
        {
            Collection_Data item = PropDistributor.Instance.DistributeRandomCollectionbyLevel(level);
            if (item)
                StartCoroutine(item.OnDistributed(currentRoom.centerPosition.position, currentRoom.centerPosition.position + new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))));
        }
    }

    /// <summary>
    /// 掉落指定藏品
    /// </summary>
    /// <param name="collection">指定藏品</param>
    /// <param name="isDirectlyAdd">是否直接添加到玩家背包</param>
    public void DropCollection(ObtainableObjectData collection, bool isDirectlyAdd)
    {
        if (collection is not Collection_Data)
            return;

        if (isDirectlyAdd)
        {
            PropDistributor.Instance.DistributeColection(currentRoom.centerPosition.position, GameObject.FindGameObjectWithTag("Player").transform.position, collection as Collection_Data);
        }
        else
        {
            ObtainableObjectData item = Instantiate(collection);
            StartCoroutine(item.OnDistributed(currentRoom.centerPosition.position, currentRoom.centerPosition.position + new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))));
        }
    }

    /// <summary>
    /// 掉落指定等级的道具
    /// </summary>
    /// <param name="level">道具等级</param>
    public void DropProp(int level)
    {
        Prop_Data item = PropDistributor.Instance.DistributeRandomPropbyLevel(level);
        if (item)
            StartCoroutine(item.OnDistributed(currentRoom.centerPosition.position, currentRoom.centerPosition.position + new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))));
    }

    /// <summary>
    /// 掉落指定道具
    /// </summary>
    /// <param name="prop">指定道具</param>
    public void DropProp(ObtainableObjectData prop)
    {
        if (prop is not Prop_Data)
            return;

        ObtainableObjectData item = Instantiate(prop);
        StartCoroutine(item.OnDistributed(currentRoom.centerPosition.position, currentRoom.centerPosition.position + new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))));
    }
}
