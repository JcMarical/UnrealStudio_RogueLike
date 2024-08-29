using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsUI : MonoBehaviour
{
    private Image[] healthUI;
    private CanvasGroup canvasGroup;
    private float currentHealth;
    private float currentMaxHealth;

    private void Start()
    {
        healthUI = new Image[100];
        currentHealth = Player.Instance.RealPlayerHealth;
        currentMaxHealth = Player.Instance.RealMaxHealth;

        if(gameObject.name.Equals("Player_Image"))
        {
            Player.Instance.dashAlpha += DashAlphaSetting;
            Image image = transform.GetComponent<Image>();
            image.sprite = Player.Instance.UISprite;
            canvasGroup= GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }

        if (gameObject.name.Equals("HPpanel"))
        {
            Player.Instance.generateHeart += ChangeHealthNum;
            Player.Instance.healthChanging += ShowHealth;
            for(int i=0;i<transform.childCount;i++) 
            {
                healthUI[i]=transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>();
            }
        }
    }

    private void OnDisable()
    {
        if (gameObject.name.Equals("Player_Image"))
        {
            Player.Instance.dashAlpha -= DashAlphaSetting;
        }

        if (gameObject.name.Equals("HPpanel"))
        {
            Player.Instance.generateHeart -= ChangeHealthNum;
            Player.Instance.healthChanging -= ShowHealth;
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
        for(int i=num;i<(int)(Player.Instance.RealMaxHealth/10);i++)
        {

            healthUI[i].fillAmount = 0;
        }

        if(num>=(int)(Player.Instance.RealMaxHealth/ 10))
        {
            return;
        }
        if((rest>=5&&currentHealth - health < 0)||( rest>0&&rest<= 5 && currentHealth - health > 0))
        {
            healthUI[num].fillAmount = 0.5f;
        }
        if(rest > 5&&currentHealth - health > 0)
        {
            healthUI[num].fillAmount = 1f;
        }
        if (rest < 5 && currentHealth - health < 0)
        {
            healthUI[num].fillAmount = 0f;
        }

        currentHealth =health;
    }

    public void ChangeHealthNum(float health)
    {
        if (transform.childCount == health / 10)
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            healthUI[transform.childCount - 1].fillAmount = 0;
            currentMaxHealth = health;
            return;
        }

        int num = (int)(health / 10 - transform.childCount);
        if(num>0)
        {
            var obj = Resources.Load<GameObject>("Player/HP");
            for (int i = 0; i < num; i++)
            {
                GameObject heart = Instantiate(obj);
                heart.transform.SetParent(transform,false);
                healthUI[transform.childCount-1] = transform.GetChild(transform.childCount - 1).GetChild(0).gameObject.GetComponent<Image>();
                healthUI[transform.childCount - 1].fillAmount = 0;
            }
        }
        else
        {
            if (currentMaxHealth - health < 0)
            {
                for (int i = (int)(currentMaxHealth / 10); i < (int)(health / 10); i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    healthUI[i].fillAmount = 0;
                }
            }
            else
            {
                for (int i = (int)(health / 10); i < (int)(currentMaxHealth / 10); i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                    healthUI[i].fillAmount = 1;
                }
            }
        }
        currentMaxHealth = health;
    }
}
