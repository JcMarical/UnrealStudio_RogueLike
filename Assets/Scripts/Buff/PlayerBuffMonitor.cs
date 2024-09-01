using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuffMonitor : TInstance<PlayerBuffMonitor>
{
    private Text AtkValue_Text;
    private Text AtkRange_Text;
    private Text AtkSpeed_Text;
    private Text MoveSpeed_Text;
    private Text Weight_Text;
    private Text Lucky_Text;
    private Text Anxiety_Text;

    private float injury_buff=1;
    private float atk_value_buff=1;
    private float atk_range_buff=1;
    private float atk_speed_buff=1;
    private float move_speed_buff=1;
    private float current_weight=1;
    private float lucky;
    private float anxiety;

    public event Action PlayerBuffUpdated;

    protected override void Awake()
    {
        base.Awake();
        PlayerBuffUpdated += DataUpdate;
        //UIInit();
        DataUpdate();
    }

    private void UIInit()
    {
        AtkValue_Text = transform.GetChild(0).GetComponent<Text>();
        AtkRange_Text = transform.GetChild(1).GetComponent<Text>();
        AtkSpeed_Text = transform.GetChild(2).GetComponent<Text>();
        MoveSpeed_Text = transform.GetChild(3).GetComponent<Text>();
        Weight_Text = transform.GetChild(4).GetComponent<Text>();
        Lucky_Text = transform.GetChild(5).GetComponent<Text>();
        Anxiety_Text = transform.GetChild(6).GetComponent<Text>();
    }

    /// <summary>
    /// ���ݺ���ʾ�����
    /// </summary>
    private void DataUpdate()
    {
        //uapdate weapon data
        UpdateWeaponData(WeaponCtrl.Instance.GetWeaponData());
        //Update player data
        UpdatePlayerData();
        //Update UI panel
        UpdateUI();
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="weaponDatas"></param>
    private void UpdateWeaponData(List<WeaponData> weaponDatas)
    {
        WeaponCtrl.Instance.GetFacWeaponData().DamageValue_fac *= atk_value_buff * injury_buff; 
        WeaponCtrl.Instance.GetFacWeaponData().AttachRadius_fac *= atk_range_buff;
    }

    private void UpdatePlayerData()
    {
        Player.Instance.attackInterval *= atk_speed_buff;
        Player.Instance.RealPlayerSpeed *= move_speed_buff;
    }

    /// <summary>
    /// Upate UI which shows the player's buff
    /// </summary>
    private void UpdateUI()
    { 
        
    }

    public float AtkValueBuff
    {
        get => atk_value_buff;
        set
        {
            atk_value_buff = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float AtkRangeBuff
    {
        get => atk_range_buff;
        set
        {
            atk_range_buff = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float AtkSpeedBuff
    {
        get => atk_speed_buff;
        set
        {
            atk_speed_buff = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float MoveSpeedBuff
    {
        get => move_speed_buff;
        set
        {
            move_speed_buff = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float CurrentWeight
    {
        get => current_weight;
        set
        {
            current_weight = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float Lucky
    {
        get => lucky;
        set
        {
            lucky = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float Anxiety
    {
        get => anxiety;
        set
        {
            anxiety = value;
            PlayerBuffUpdated?.Invoke();
        }
    }

    public float InjuryBuff
    {
        get => injury_buff;
        set
        {
            injury_buff = value;
            PlayerBuffUpdated?.Invoke();
        }
    }
}