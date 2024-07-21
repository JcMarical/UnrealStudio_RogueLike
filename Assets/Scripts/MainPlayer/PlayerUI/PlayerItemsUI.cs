using MainPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsUI : MonoBehaviour
{
    public static Action<float> dashAlpha;
    public static Action<float> healthUp;
    public static Action<float> healthDown;
    private Image[] healthUI;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        healthUI = new Image[transform.childCount];
        if(gameObject.name.Equals("PlayerPicture"))
        {
            dashAlpha += DashAlphaSetting;
            canvasGroup= GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }
        if (gameObject.name.Equals("Heart"))
        {
            healthUp += IncreaseHp;
            healthDown += ReduceHp;
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


    public void IncreaseHp(float health)
    {
        if (Player.Instance.realPlayerHealth >= 100)
        {
            healthUI[9].fillAmount = 1;
            return;
        }

        float pre = Player.Instance.realPlayerHealth-health;
        float cur = Player.Instance.realPlayerHealth;
        int increaseNum = (int)(cur / 5) - (int)(pre / 5), baseNum = (int)(pre / 10);
        if (pre % 5 == 0 && cur % 5 != 0)
        {
            increaseNum += 1;
        }
        if (pre % 5 != 0 && cur % 5 == 0)
        {
            increaseNum -= 1;
        }

        for (int i=0;i< increaseNum;i++)
        {
            if (healthUI[baseNum].fillAmount!=1)
            {
                healthUI[baseNum].fillAmount += 0.5f;
            }
            else
            {
                baseNum += 1;
                healthUI[baseNum].fillAmount += 0.5f;
            }
        }
    }

    public void ReduceHp(float health)
    {
        if (Player.Instance.realPlayerHealth <= 0)
        {
            healthUI[0].fillAmount = 0;
            return;
        }

        float pre = Player.Instance.realPlayerHealth + health;
        float cur=Player.Instance.realPlayerHealth;
        int increaseNum=(int)(pre/ 5)-(int)(cur/5),baseNum= (int)(pre / 10);

        if (pre % 5 == 0 && cur % 5 != 0)
        {
            increaseNum -= 1;
        }
        if (pre % 5 != 0 && cur % 5 == 0)
        {
            increaseNum += 1;
        }
        if(baseNum==10)
        {
            baseNum = 9;
        }

        for (int i = 0; i < increaseNum; i++)
        {
            if (healthUI[baseNum].fillAmount != 0)
            {
                healthUI[baseNum].fillAmount -= 0.5f;
            }
            else
            {
                baseNum -= 1;
                healthUI[baseNum].fillAmount -= 0.5f;
            }
        }
    }
}
