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
}

public class EventRoomMgr : TInstance<EventRoomMgr>
{
    public EventData currentEvent;
    public List<EventData> eventList;

    private Sequence sequence;

    [Serializable]
    public struct Choice
    {
        public RectTransform buttonTransform;
        public Text title;
        public Text description;
    }

    private bool isPlay;
    public int choiceNumber;
    private int phase;

    [Header("UI")]
    public RectTransform canvas;
    public GameObject EventRoomUI;
    public Image backgroundImage;
    public Text eventTitle;
    public RectTransform backgroundAndTitle;
    private string words;
    public Text eventDescription;
    [Space(16)]
    public Choice[] choices;
    [Space(16)]
    public RectTransform closeButton;

    [Header("FSM")]
    private EventState currentState;
    private EventState innocentLambState;

    protected override void Awake()
    {
        base.Awake();

        innocentLambState = new InnocentLambState();

        closeButton.GetComponent<Button>().onClick.AddListener(CloseMenu);
        (closeButton.GetComponent(typeof(Button)) as Button).onClick.AddListener(CloseMenu);
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
            _ => null
        };

        currentState.OnEnter();
    }

    public void ExitState()
    {
        currentState.OnExit();
        currentState = null;
    }

    [ContextMenu("进入随机事件")]
    public void EnterEvent()
    {
        //禁用玩家操作
        //BindingChange.Instance.inputControl.Disable();
        //Player.Instance.GetComponentInChildren<PlayerAnimation>().inputControl.Disable();

        //随机抽取事件
        EventData randomEvent = eventList[UnityEngine.Random.Range(0, eventList.Count)];
        currentEvent = randomEvent;
        if (!randomEvent.isRepeatable)
            eventList.Remove(randomEvent);

        //初始化文本数据
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

    public void ContinueEvent()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].buttonTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            choices[i].buttonTransform.parent.gameObject.SetActive(false);
        }

        words = currentEvent.choices[choiceNumber].result;
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

    private void CloseMenu()
    {
        closeButton.gameObject.SetActive(false);
        EventRoomUI.SetActive(false);
        //BindingChange.Instance.inputControl.Enable();
        //Player.Instance.GetComponentInChildren<PlayerAnimation>().inputControl.Enable();
    }


}
