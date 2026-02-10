using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        FindChildrenUiBehaviours<Button>();
        FindChildrenUiBehaviours<TextMeshProUGUI>();
        FindChildrenUiBehaviours<Slider>();
    }

    private void Start()
    {
        GetUiBehaviour<Button>("Continue").onClick.AddListener(ContinueBtnAction);
        GetUiBehaviour<Button>("Restart").onClick.AddListener(RestartBtnAction);
        GetUiBehaviour<Button>("Return").onClick.AddListener(ReturnBtnAction);
        GetUiBehaviour<Slider>("Volume").onValueChanged.AddListener(VolumeSliderAction);
        
    }

    private void OnDestroy()
    {
        GetUiBehaviour<Button>("Continue").onClick.RemoveAllListeners();
        GetUiBehaviour<Button>("Restart").onClick.RemoveAllListeners();
        GetUiBehaviour<Button>("Return").onClick.RemoveAllListeners();
        GetUiBehaviour<Slider>("Volume").onValueChanged.RemoveAllListeners();
    }

    private void ContinueBtnAction()
    {
        Time.timeScale = 1f;
        InputManager.GetInstance().inputMaps.Enable();
        UiManager.GetInstance().HidePanel("PauseUI");
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
    
    private void VolumeSliderAction(float volume)
    {
        AudioManager.GetInstance().ChangeSoundVolume(volume);
    }
}
