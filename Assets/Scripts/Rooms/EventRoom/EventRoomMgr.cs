using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI")]
    public RectTransform canvas;
    public Image backgroundImage;
    public Text eventTitle;
    public RectTransform backgroundAndTitle;
    private string words;
    public Text eventDescription;
    public Choice[] choices;

    protected override void Awake()
    {
        base.Awake();
    }

    [ContextMenu("进入随机事件")]
    public void EnterEvent()
    {
        EventData randomEvent = eventList[UnityEngine.Random.Range(0, eventList.Count)];
        currentEvent = randomEvent;
        if (!randomEvent.isRepeatable)
            eventList.Remove(randomEvent);

        backgroundImage = currentEvent.backgroundImage;
        eventTitle.text = currentEvent.eventTitle;
        words = currentEvent.eventDescription;
        eventDescription.text = null;
        for (int i = 0; i < choices.Length; i++)
            choices[i].bottonTransform.gameObject.SetActive(false);
        for (int i = 0; i < currentEvent.choices.Length; i++)
        {
            choices[i].title.text = currentEvent.choices[i].title;
            choices[i].description.text = currentEvent.choices[i].description;
            choices[i].bottonTransform.gameObject.SetActive(true);
        }

        sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.Append(backgroundAndTitle.DOMove(new Vector3(canvas.rect.width * 0.5f * canvas.lossyScale.x, canvas.rect.height * 0.5f * canvas.lossyScale.y, 0), 2));
        sequence.Append(eventDescription.DOText(words, words.Length * 0.05f));

        sequence.Play();
    }
}
