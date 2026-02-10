using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        FindChildrenUiBehaviours<TextMeshProUGUI>();
        FindChildrenUiBehaviours<Button>();
    }

    void Start()
    {
        GetUiBehaviour<Button>("Restart").onClick.AddListener(RestartBtnAction);
        GetUiBehaviour<Button>("Return").onClick.AddListener(ReturnBtnAction);
    }

    private void OnEnable()
    {
        //当此ui被实例化时，肯定游戏结束了
        GameOverOnUIAction();
    }


    private void OnDestroy()
    {
        GetUiBehaviour<Button>("Restart").onClick.RemoveAllListeners();
        GetUiBehaviour<Button>("Return").onClick.RemoveAllListeners();
    }

    private void RestartBtnAction()
    {
        Time.timeScale = 1f;
        InputManager.GetInstance().inputMaps.Enable();
        SceneChangeManager.GetInstance().LoadSceneAsync("GameScene",()=>{});
    }

    private void ReturnBtnAction()
    {
        Time.timeScale = 1f;
        InputManager.GetInstance().inputMaps.Enable();
        SceneChangeManager.GetInstance().LoadSceneAsync("MainMenuScene",()=>{});
    }
    
    //游戏结束时调用
    private void GameOverOnUIAction()
    {
        Time.timeScale = 0;
        GetUiBehaviour<TextMeshProUGUI>("Score").text = "Your Score:" + DataManager.GetInstance().Score;
        GetUiBehaviour<TextMeshProUGUI>("BestScore").text = "Best Score:" + DataManager.GetInstance().BestScore;
    }
}
