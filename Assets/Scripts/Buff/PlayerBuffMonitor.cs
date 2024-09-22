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

    private float injury_buff = 1;
    private float atk_value_buff = 1;
    private float atk_range_buff = 1;
    private float atk_speed_buff = 1;
    private float move_speed_buff = 1;
    private float current_weight = 1;
    private float lucky;
    private float anxiety;

    public Action playerBuffUpdated;

    public event Action PlayerBuffUpdated
    {
        add
        {
            if (playerBuffUpdated == null || Array.IndexOf(playerBuffUpdated.GetInvocationList(), value) == -1)
            {
                playerBuffUpdated += value;
            }
        }
        remove
        {
            playerBuffUpdated -= value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        PlayerBuffUpdated += DataUpdate;
    }
    // private void UIInit()
    // {
    //     AtkValue_Text = transform.GetChild(0).GetComponent<Text>();
    //     AtkRange_Text = transform.GetChild(1).GetComponent<Text>();
    //     AtkSpeed_Text = transform.GetChild(2).GetComponent<Text>();
    //     MoveSpeed_Text = transform.GetChild(3).GetComponent<Text>();
    //     Weight_Text = transform.GetChild(4).GetComponent<Text>();
    //     Lucky_Text = transform.GetChild(5).GetComponent<Text>();
    //     Anxiety_Text = transform.GetChild(6).GetComponent<Text>();
    // }

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
            if (atk_value_buff != value)
            {
                atk_value_buff = value;
                playerBuffUpdated?.Invoke();
                atk_value_buff =1;
                AtkValueBuff = 1;
            } 
        }
    }

    public float AtkRangeBuff
    {
        get => atk_range_buff;
        set
        {
            if (atk_range_buff != value)
            {
                atk_range_buff = value;
                playerBuffUpdated?.Invoke();
                atk_range_buff = 1;
                AtkRangeBuff = 1;
            } 
        }
    }

    public float AtkSpeedBuff
    {
        get => atk_speed_buff;
        set
        {
            if (atk_speed_buff != value)
            {
                atk_speed_buff = value;
                playerBuffUpdated?.Invoke();
                atk_speed_buff = 1;
                AtkSpeedBuff = 1;
            }
        }
    }

    public float MoveSpeedBuff
    {
        get => move_speed_buff;
        set
        {
            if (move_speed_buff != value)
            {
                move_speed_buff = value;
                playerBuffUpdated?.Invoke();
                move_speed_buff = 1;
                MoveSpeedBuff = 1;
            }
        }
    }

    public float CurrentWeight
    {
        get => current_weight;
        set
        {
            if (current_weight != value)
            {
                current_weight = value;
                playerBuffUpdated?.Invoke();
            } 
        }
    }

    public float Lucky
    {
        get => lucky;
        set
        {
            if (lucky != value)
            {
                lucky = value;
                playerBuffUpdated?.Invoke();
            } 
        }
    }

    public float Anxiety
    {
        get => anxiety;
        set
        {
            if (anxiety != value)
            {
                anxiety = value;
                playerBuffUpdated?.Invoke();
            }
        }
    }

    public float InjuryBuff
    {
        get => injury_buff;
        set
        {
            if (injury_buff != value)
            {
                injury_buff = value;
                playerBuffUpdated?.Invoke();
            } 
        }
    }
}