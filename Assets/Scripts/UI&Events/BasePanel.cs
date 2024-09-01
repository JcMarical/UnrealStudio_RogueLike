using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasePanel : MonoBehaviour
{
    public Canvas canvas;
    public GameObject NowPanel;
    #region UI������Ҫֱ�Ӷ�ȡ�����
    public MainPlayer.Player player;
    public PlayerSS_FSM playerSS_FSM;
    public PropBackPackUIMgr propBackPackUIMgr;
    #endregion

    public void Initialization()
    {
        player = MainPlayer.Player.Instance;
        playerSS_FSM = GameObject.FindWithTag("Player").GetComponent<PlayerSS_FSM>();
        propBackPackUIMgr = PropBackPackUIMgr.Instance;
    }
    public virtual void OnEnable()
    {

    }
    public virtual void OnDisable()
    {

    }
    public virtual void OnDestory()
    {

    }
        
}
