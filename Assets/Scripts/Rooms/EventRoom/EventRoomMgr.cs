using DG.Tweening;
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
        public RectTransform bottonTransform;
        public Text title;
        public Text description;
    }

    private bool isPlay;

    [Header("UI")]
    public RectTransform canvas;
    public GameObject EventRoomUI;
    public Image backgroundImage;
    public Text eventTitle;
    public RectTransform backgroundAndTitle;
    private string words;
    public Text eventDescription;
    public Choice[] choices;

    [Header("FSM")]
    private EventState currentState;
    private EventState innocentLambState;

    protected override void Awake()
    {
        base.Awake();

        innocentLambState = new InnocentLambState();
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
        for (int i = 0; i < choices.Length; i++)
            choices[i].bottonTransform.parent.gameObject.SetActive(false);
        for (int i = 0; i < currentEvent.choices.Length; i++)
        {
            choices[i].title.text = currentEvent.choices[i].title;
            choices[i].description.text = currentEvent.choices[i].description;
            choices[i].bottonTransform.parent.gameObject.SetActive(true);
        }

        //初始化UI
        backgroundAndTitle.position = new Vector3(backgroundAndTitle.position.x, canvas.rect.height * canvas.lossyScale.y * 2.5f, 0);
        for (int i = 0; i < currentEvent.choices.Length; i++)
            choices[i].bottonTransform.position = new Vector3(canvas.rect.width * canvas.lossyScale.x * 1.2f, choices[i].bottonTransform.position.y, 0);

        EventRoomUI.SetActive(true);

        //设置UI动画
        sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.Append(backgroundAndTitle.DOMoveY(canvas.rect.height * canvas.lossyScale.y * 0.5f, 2));
        sequence.Append(eventDescription.DOText(words, words.Length * 0.05f));
        for (int i = 0; i < currentEvent.choices.Length; i++)
            sequence.Append(choices[i].bottonTransform.DOMoveX(canvas.rect.width * canvas.lossyScale.x * 0.8f, 1));
        sequence.AppendCallback( () => isPlay = false );

        sequence.Play();
        isPlay = true;
    }

    private void SkipAnimation()
    {
        sequence.Kill();

        backgroundAndTitle.position = new Vector3(backgroundAndTitle.position.x, canvas.rect.height * canvas.lossyScale.y * 0.5f, 0);
        eventDescription.text = words;
        for (int i = 0; i < currentEvent.choices.Length; i++)
            choices[i].bottonTransform.position = new Vector3(canvas.rect.width * canvas.lossyScale.x * 0.8f, choices[i].bottonTransform.position.y, 0);
    }
}
