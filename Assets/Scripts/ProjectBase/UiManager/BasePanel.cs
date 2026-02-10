using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类
/// 获取ui面板上的所有组件
/// </summary>
public class BasePanel : MonoBehaviour
{
    //使用字典，根据名称存储ui控件
    private Dictionary<string, UIBehaviour> _uiBehaviours = new Dictionary<string, UIBehaviour>();

    protected virtual void Awake()
    {
        //在awake中使用FindChildrenUiBehaviours<T>获取需要的组件并存储在 字典容器 中
    }

    /// <summary>
    /// 初始化遍历搜寻子对象UI控件并存储到字典中
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
    protected void FindChildrenUiBehaviours<T>() where T : UIBehaviour
    {
        T[] uiBehaviours = GetComponentsInChildren<T>();
        foreach (T uiBehaviour in uiBehaviours)
        {
            //由于一个game object对象可以有多个ui控件，通过 对象名+控件名 的方式来区分存储
            _uiBehaviours.Add(uiBehaviour.gameObject.name + typeof(T), uiBehaviour);
        }
    }

    /// <summary>
    /// 用于获取ui控件
    /// </summary>
    /// <param name="behaviourName">控件挂载的GameObject对象名</param>
    /// <typeparam name="T">控件类型</typeparam>
    /// <returns></returns>
    public T GetUiBehaviour<T>(string behaviourName) where T : UIBehaviour
    {
        if (_uiBehaviours.ContainsKey(behaviourName + typeof(T)))
        {
            return _uiBehaviours[behaviourName + typeof(T)] as T;
        }
        else
        {
            return null;
        }
    }
}
