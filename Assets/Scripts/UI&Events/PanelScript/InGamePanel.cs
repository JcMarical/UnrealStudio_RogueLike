using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{


    //TODO:玩家异常读取与显示

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
    public List<Image> NeverChangeImages;
    public Image[] HeahthImages;
    #region 属性值显示更新
    private void ChangeValue(Property property, float changedValue)
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
        ChangeValue(Property.AtkSpeed, value);
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

    #region 玩家生命值显示

    #endregion

    #region 异常显示栏
    public RectTransform SS_ListCenter;
    public HorizontalLayoutGroup layoutGroup;
    [SerializeField] int SS_quantity;
    public Image[] SS_list;
    private void SpecialStateUI(List<SpecialState> StatesList)
    {
        SS_quantity = StatesList.Count;
        //用于保证每个图标中心都是状态显示栏的x+1等分点
        layoutGroup.spacing = SS_ListCenter.rect.width / (StatesList.Count + 1) - SS_list[0].rectTransform.rect.width;
        Debug.Log(layoutGroup.spacing);
        for (int i = 0; i < SS_list.Length; i++)
        {
            if (i < SS_quantity)
            {
                SS_list[i].gameObject.SetActive(true);
                SS_list[i].sprite = StatesList[i].Sprite;
            }
            else
            {
                SS_list[i].gameObject.SetActive(false);
            }
        }
    }

    private void StateUI()
    {
        SpecialStateUI(playerSS_FSM.GetCurrentState());
    }
    #endregion

    public override void OnEnable()
    {
        base.OnEnable();

        propBackPackUIMgr.enabled = true;

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

        playerSS_FSM.WhenStateEnter += StateUI;

        SpecialStateUI(playerSS_FSM.GetCurrentState());
    }


    public override void OnDisable()
    {
        base.OnDisable();

        propBackPackUIMgr.enabled= false;

        player.playerAttackChanging -= ChangeAtkValue;
        player.playerRangeChanging -= ChangeAtkRange;
        player.attackSpeedChanging -= ChangeAtkSpeed;
        player.playerSpeedChanging -= ChangeMoveSpeed;
        player.luckyChanging -= ChangeLucky;
        player.unluckyChanging -= ChangeAnxiety;

        playerSS_FSM.WhenStateEnter -= StateUI;
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
