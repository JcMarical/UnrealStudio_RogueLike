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

    private float atk_value_buff;
    private float atk_range_buff;
    private float atk_speed_buff;
    private float move_speed_buff;
    private float current_weight;
    private float lucky;
    private float anxiety;

    public event Action PlayerBuffUpdated;

    protected override void Awake()
    {
        base.Awake();
        PlayerBuffUpdated += DataUpdate;
        UIInit();
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
    /// 数据和显示层更新
    /// </summary>
    private void DataUpdate()
    {
        //武器数据更新
        UpdateWeaponData(WeaponCtrl.Instance.GetWeaponData());
        //玩家数据更新
        UpdatePlayerData();
        //UI更新
        UpdateBuffUI();
    }

    /// <summary>
    /// 更新武器数据
    /// </summary>
    /// <param name="weaponDatas"></param>
    private void UpdateWeaponData(List<WeaponData> weaponDatas)
    {
        
    }

    private void UpdatePlayerData()
    { 
    
    }

    /// <summary>
    /// 更新左边buff数据栏
    /// </summary>
    private void UpdateBuffUI()
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
}