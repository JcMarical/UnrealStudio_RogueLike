using MainPlayer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributesUI:MonoBehaviour
{
    private string str;
    private void Start()
    {
        Initial();
    }

    private void Initial()
    {
        var t = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        str = gameObject.name;
        
        switch(str)
        {
            case "MoveSpeed": t.text = Player.Instance.realPlayerSpeed.ToString();
                break;
            case "Lucky": t.text = Player.Instance.realLucky.ToString();
                break;
            case "Unlucky":
                t.text = Player.Instance.realUnlucky.ToString();
                break;

        }
    }

    private void Update()
    {
        str = gameObject.name;

        switch (str)
        {
            case "MoveSpeed":
                ShowingAttributes(() => Player.Instance.realPlayerSpeed);
                break;
            case "Lucky":
                ShowingAttributes(() => Player.Instance.realLucky);
                break;
            case "Unlucky":
                ShowingAttributes(() => Player.Instance.realUnlucky);
                break;

        }
    }

    public void ShowingAttributes(Func<float> func)
    {
        float value = func();
        var t = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (gameObject.tag.Equals(str))
        {
            t.text = ((int)value).ToString();
        }
    }
}
