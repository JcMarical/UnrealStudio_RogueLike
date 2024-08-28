using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName ="InGamePanel",menuName ="Data/UIPanel/InGamePanel",order = 0)]
public class InGamePanel : BasePanel
{
    [Header("Texts")]
    public TextMeshProUGUI AtkValue_text;
    public TextMeshProUGUI AtkRange_text;
    public TextMeshProUGUI AtkSpeed_text;
    public TextMeshProUGUI MoveSpeed_text;
    public TextMeshProUGUI Lucky_text;
    public TextMeshProUGUI Anxiety_text;
    //public TextMeshProUGUI 
    [Header("Buttons")]
    [Header("Images")]
    public List<Image> images;

    private void ChangeValue(Property property,float changedValue)
    {
        switch (property)
        {
            case Property.AtkValue:
                AtkValue_text.text = changedValue.ToString(); break;
            case Property.AtkSpeed:
                AtkRange_text.text = changedValue.ToString(); break;
            case Property.AtkRange:
                AtkRange_text.text = changedValue.ToString(); break;
            case Property.MoveSpeed:
                MoveSpeed_text.text = changedValue.ToString(); break;
            case Property.Lucky:
                Lucky_text.text = changedValue.ToString(); break;
            case Property.Anxiety:
                Anxiety_text.text = changedValue.ToString(); break;
        }

    }

    public override void OnEnable()
    {
        base.OnEnable();
        AtkValue_text.text = player.realPlayerAttack.ToString();
        AtkRange_text.text = player.realPlayerRange.ToString();
        AtkSpeed_text.text = player.realAttackSpeed.ToString();
        MoveSpeed_text.text = player.realPlayerSpeed.ToString();
        Lucky_text.text = player.realLucky.ToString();
        Anxiety_text.text = player.realUnlucky.ToString();

        UIManager.Instance.ChangePropertyEvent += ChangeValue;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        UIManager.Instance.ChangePropertyEvent -= ChangeValue;
    }

}

public enum Property
{
    AtkValue,
    AtkRange,
    AtkSpeed,
    MoveSpeed,
    Lucky,
    Anxiety,
}
