using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName ="InGamePanel",menuName ="Data/UIPanel/InGamePanel",order = 0)]
public class InGamePanel : BasePanel
{
    [Header("Texts")]
    public TextMeshPro AtkValue_text;
    public TextMeshPro AtkRange_text;
    public TextMeshPro AtkSpeed_text;
    [Header("Buttons")]
    public Button a;
}
