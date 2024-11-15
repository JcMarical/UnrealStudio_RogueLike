using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Better : MonoBehaviour
{
    public Text uiText;

    void Start()
    {
        if (uiText == null)
        {
            uiText = GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text="连击："+Whole.Batter.ToString();
    }
}
