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
    public PropBackPackUIMgr propBackPackUIMgr;
    #endregion

    public void Initialization()
    {
        player = MainPlayer.Player.Instance;
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
