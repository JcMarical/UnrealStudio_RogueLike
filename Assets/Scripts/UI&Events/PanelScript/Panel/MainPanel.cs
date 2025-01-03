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
    Button NewGameButton;
    Button ContinueButton;
    GameObject StartPanel;
    public CanvasGroup NewGamePanel;
    public CanvasGroup ContinuePanel;

    public override void OnEnable()
    {
        base.OnEnable();
        DOTween.Init();
        StartGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        NewGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        ContinueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        GOButton = GameObject.Find("GO").GetComponent<Button>();
        StartPanel = GameObject.Find("StartGamePanel");
        StartGameButton.onClick.AddListener(GameStart);
        GOButton.onClick.AddListener(SceneChange);
        NewGameButton.onClick.AddListener(NewGame);
        ContinueButton.onClick.AddListener(GameContinue);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StartGameButton.onClick.RemoveListener(GameStart);
        GOButton.onClick.RemoveAllListeners();
        NewGameButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.RemoveAllListeners();
    }

    private void GameStart()
    {
        StartPanel.transform.DOMoveY(600, 0.5F, false);
    }
    private void SceneChange()
    {
        SceneManager.LoadSceneAsync("Demo");
    }

    private void NewGame()
    {
        NewGamePanel.DOFade(1, 0.6F);
        ContinuePanel.DOFade(0,0.6f);
        NewGamePanel.interactable = true;
        ContinuePanel.interactable = false;
    }

    private void GameContinue() 
    {
        NewGamePanel.DOFade(0, 0.6F);
        ContinuePanel.DOFade(1, 0.6f);
        NewGamePanel.interactable = false;
        ContinuePanel.interactable = true;
    }
}
