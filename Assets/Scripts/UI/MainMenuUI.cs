using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        FindChildrenUiBehaviours<Button>();
        FindChildrenUiBehaviours<Slider>();
    }

    private void Start()
    {
        GetUiBehaviour<Button>("Start").onClick.AddListener(StartBtnAction);
        GetUiBehaviour<Button>("Quit").onClick.AddListener(QuitBtnAction);
        GetUiBehaviour<Slider>("Volume").onValueChanged.AddListener(VolumeSliderAction);
    }

    private void OnDestroy()
    {
        GetUiBehaviour<Button>("Start").onClick.RemoveAllListeners();
        GetUiBehaviour<Button>("Quit").onClick.RemoveAllListeners();
        GetUiBehaviour<Slider>("Volume").onValueChanged.RemoveAllListeners();
    }

    private void StartBtnAction()
    {
        SceneChangeManager.GetInstance().LoadSceneAsync("GameScene", () => { });
    }

    private void QuitBtnAction()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void VolumeSliderAction(float volume)
    {
        AudioManager.GetInstance().ChangeSoundVolume(volume);
    }
}
