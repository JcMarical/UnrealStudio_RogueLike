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
            case "MoveSpeed":
                t.text = Player.Instance.RealPlayerSpeed.ToString();
                break;
            case "Lucky": t.text = Player.Instance.RealLucky.ToString();
                break;
            case "Unlucky":
                t.text = Player.Instance.RealUnlucky.ToString();
                break;

        }
    }

    private void Update()
    {
        if (Player.Instance)
        {
            str = gameObject.name;

            switch (str)
            {
                case "MoveSpeed":
                    ShowingAttributes(() => Player.Instance.RealPlayerSpeed);
                    break;
                case "Lucky":
                    ShowingAttributes(() => Player.Instance.RealLucky);
                    break;
                case "Unlucky":
                    ShowingAttributes(() => Player.Instance.RealUnlucky);
                    break;

            }
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
