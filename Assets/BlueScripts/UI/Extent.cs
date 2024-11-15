using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extent : MonoBehaviour
{
    public Text uiText;  // 引用 UI Text 组件

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
        if(Whole.NoteExtent.Count > 0)
        {
            uiText.text = Whole.NoteExtent[Whole.NoteExtent.Count-1];
        }
    }
}
