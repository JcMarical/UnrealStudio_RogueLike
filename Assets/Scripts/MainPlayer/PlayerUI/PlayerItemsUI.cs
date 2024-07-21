using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsUI : MonoBehaviour
{
  
    public static Action<float> healthUp;
    public static Action<float> healthDown;
    private Image[] healthUI;
    private CanvasGroup canvasGroup;
    private float currentHealth;

    private void Start()
    {
        healthUI = new Image[100];
        currentHealth = Player.Instance.realPlayerHealth;

        if(gameObject.name.Equals("PlayerPicture"))
        {
            Player.dashAlpha += DashAlphaSetting;
            canvasGroup= GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }

        if (gameObject.name.Equals("Heart"))
        {
            Player.GenerateHeart += ChangeHealthNum;
            Player.healthChanging += ShowHealth;
            for(int i=0;i<transform.childCount;i++) 
            {
                healthUI[i]=transform.GetChild(i).gameObject.GetComponent<Image>();
            }
        }
    }

    public void DashAlphaSetting(float alpha)
    {
        canvasGroup.alpha = 0.2f + alpha / 1.25f;
    }
    public void ShowHealth(float health)
    {
        int num = (int)(health / 10);
        int rest = (int)(health % 10);
        for(int i=0;i<num;i++)
        {
            healthUI[i].fillAmount = 1;
        }
        for(int i=num;i<Player.Instance.realMaxHealth/10;i++)
        {
            healthUI[i].fillAmount = 0;
        }

        if(num>= Player.Instance.realMaxHealth/ 10)
        {
            return;
        }
        if((rest>=5&&currentHealth-Player.Instance.realPlayerHealth<0)||( rest>0&&rest<= 5 && currentHealth - Player.Instance.realPlayerHealth >0))
        {
            healthUI[num].fillAmount = 0.5f;
        }
        if(rest > 5&&currentHealth - Player.Instance.realPlayerHealth > 0)
        {
            healthUI[num].fillAmount = 1f;
        }
        if (rest < 5 && currentHealth - Player.Instance.realPlayerHealth < 0)
        {
            healthUI[num].fillAmount = 0f;
        }

        currentHealth =health;
    }

    public void ChangeHealthNum(float health)
    {
        if(transform.childCount==health/10)
        {
            return;
        }

        int num = (int)(health / 10 - transform.childCount);
        if(num>0)
        {
            for (int i = 0; i < num; i++)
            {
                var obj=Resources.Load<GameObject>("Player/HP");
                GameObject heart=Instantiate(obj,transform.position,Quaternion.identity);
                heart.transform.SetParent(transform);
                heart.transform.localScale = Vector3.one;
                healthUI[(int)(health / 10)-1] = transform.GetChild((int)(health / 10)-1).gameObject.GetComponent<Image>();
                healthUI[(int)(health / 10) - 1].fillAmount = 0;
            }
        }
        else
        {
            for(int i=0;i<health /10;i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            for(int i= (int)(health/ 10);i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
