using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainPanel : BasePanel
{
    [Header("按钮")]
    Button StartGameButton;
    Button GOButton;
    GameObject StartPanel; 

    public override void OnEnable()
    {
        base.OnEnable();
        DOTween.Init();
        StartGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        GOButton = GameObject.Find("GO").GetComponent<Button>();
        StartPanel = GameObject.Find("StartGamePanel");
        StartGameButton.onClick.AddListener(GameStart);
        GOButton.onClick.AddListener(SceneChange);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StartGameButton.onClick.RemoveListener(GameStart);
        GOButton.onClick.RemoveAllListeners();
    }

    private void GameStart()
    {
        StartPanel.transform.DOMoveY(600, 0.5F, false);
    }
    private void SceneChange()
    {
        SceneManager.LoadSceneAsync("Demo");
    }
}
