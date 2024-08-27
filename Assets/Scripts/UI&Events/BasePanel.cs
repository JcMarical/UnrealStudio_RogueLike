using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasePanel : ScriptableObject
{
    public Canvas canvas;
    public GameObject NowPanel;
    #region UI界面需要直接读取的组件
    public MainPlayer.Player player;
    #endregion

    public void Initialization()
    {
        player = MainPlayer.Player.Instance;
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
