using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        FindChildrenUiBehaviours<TextMeshProUGUI>();
        FindChildrenUiBehaviours<Button>();
    }

    void Start()
    {
        EventManager.GetInstance().AddEventListener<CardBase>("AddScore",AddScoreOnUI);
        GetUiBehaviour<Button>("Pause").onClick.AddListener(PauseBtnAction);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveEventListener<CardBase>("AddScore",AddScoreOnUI);
        GetUiBehaviour<Button>("Pause").onClick.RemoveAllListeners();
    }

    
    private void AddScoreOnUI(CardBase card)
    {
        StartCoroutine(AddScoreOnUICoroutine());
    }

    IEnumerator AddScoreOnUICoroutine()
    {
        //延迟一帧更新,等待数据更新完毕后再更新ui
        yield return new WaitForEndOfFrame();
        TextMeshProUGUI score = GetUiBehaviour<TextMeshProUGUI>("Score");
        if (score != null)
        {
            score.text = "Score:" + DataManager.GetInstance().Score.ToString();
        }
        yield break;
    }

    //暂停按钮相关事件
    private void PauseBtnAction()
    {
        InputManager.GetInstance().inputMaps.Disable();
        Time.timeScale = 0f;
        UiManager.GetInstance().ShowPanel("PauseUI");
    }
}
