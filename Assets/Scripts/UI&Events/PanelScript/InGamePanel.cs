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
    public List<Image> UnChangeImages;
    
    #region 属性值显示更新
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

    private void ChangeAtkValue(float value) 
    {
        ChangeValue(Property.AtkValue, value);
    }
    private void ChangeAtkSpeed(float value)
    {
        ChangeValue(Property.AtkSpeed,value);
    }
    private void ChangeAtkRange(float value)
    {
        ChangeValue(Property.AtkRange, value);
    }
    private void ChangeMoveSpeed(float value)
    {
        ChangeValue(Property.MoveSpeed, value);
    }
    private void ChangeLucky(float value)
    {
        ChangeValue(Property.Lucky, value);
    }
    private void ChangeAnxiety(float value)
    {
        ChangeValue(Property.Anxiety, value);
    }
    #endregion

    public override void OnEnable()
    {
        base.OnEnable();
        AtkValue_text.text = player.RealPlayerAttack.ToString();
        AtkRange_text.text = player.RealPlayerRange.ToString();
        AtkSpeed_text.text = player.RealAttackSpeed.ToString();
        MoveSpeed_text.text = player.RealPlayerSpeed.ToString();
        Lucky_text.text = player.RealLucky.ToString();
        Anxiety_text.text = player.RealUnlucky.ToString();

        player.playerAttackChanging += ChangeAtkValue;
        player.playerRangeChanging += ChangeAtkRange;
        player.attackSpeedChanging += ChangeAtkSpeed;
        player.playerSpeedChanging += ChangeMoveSpeed;
        player.luckyChanging += ChangeLucky;
        player.unluckyChanging += ChangeLucky;
    }


    public override void OnDisable()
    {
        base.OnDisable();
        player.playerAttackChanging -= ChangeAtkValue;
        player.playerRangeChanging -= ChangeAtkRange;
        player.attackSpeedChanging -= ChangeAtkSpeed;
        player.playerSpeedChanging -= ChangeMoveSpeed;
        player.luckyChanging -= ChangeLucky;
        player.unluckyChanging -= ChangeLucky;

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
