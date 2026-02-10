using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UI管理
/// 管理所有的面板
/// 提供给外部 显示 和 隐藏 面板
/// </summary>
public class UiManager : SingletonBase<UiManager>
{
    private Dictionary<string, BasePanel> _panels;

    public UiManager()
    {
        _panels = new Dictionary<string, BasePanel>();
        //创建EventSystem并保护不被移除
        GameObject obj = ResourceManager.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// 展示面板
    /// </summary>
    /// <param name="panelName">面板(预制体)名称</param>
    public void ShowPanel(string panelName)
    {
        if (_panels.ContainsKey(panelName))
        {
            _panels[panelName].gameObject.SetActive(true);
        }
        else
        {
            GameObject gameObject = ResourceManager.GetInstance().Load<GameObject>("UI/" + panelName);
            gameObject.SetActive(true);
            _panels.Add(panelName, gameObject.GetComponent<BasePanel>());
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">面板名称</param>
    public void HidePanel(string panelName)
    {
        if (_panels.ContainsKey(panelName))
        {
            _panels[panelName].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <param name="panelName">面板名称</param>
    public BasePanel GetPanel(string panelName)
    {
        if (_panels.ContainsKey(panelName))
        {
            return _panels[panelName];
        }
        else
        {
            GameObject gameObject = ResourceManager.GetInstance().Load<GameObject>("UI/" + panelName);
            _panels.Add(panelName,gameObject.GetComponent<BasePanel>());
            gameObject.SetActive(true);
            return _panels[panelName];
        }
    }

    //清空ui，一般在场景切换时使用
    public void ClearPanels()
    {
        _panels.Clear();
    }
}
