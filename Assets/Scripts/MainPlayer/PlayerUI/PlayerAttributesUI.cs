using MainPlayer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributesUI:MonoBehaviour
{
    public static Action<float, string> attributesChanging;
    private void Start()
    {
        Initial();
        attributesChanging += ShowingAttributes;
    }

    private void Initial()
    {
        var t = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (gameObject.name=="MoveSpeed")
        {
            t.text= Player.Instance.realPlayerSpeed.ToString();
        }
        if (gameObject.name == "Lucky")
        {
            t.text= Player.Instance.realLucky.ToString();
        }
        if (gameObject.name == "Unlucky")
        {
            t.text = Player.Instance.realUnlucky.ToString();
        }
    }

    public void ShowingAttributes(float value,string attributesName)
    {
        var t = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (gameObject.name.Equals(attributesName))
        {
            t.text = value.ToString();
        }
    }
}
