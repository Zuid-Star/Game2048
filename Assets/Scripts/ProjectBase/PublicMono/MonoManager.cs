using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 负责非mono类使用 update生命周期函数 和 协程
/// </summary>
public class MonoManager : SingletonMonoBase<MonoManager> //由于使用生命周期函数和协程，需要继承mono的单例
{
    private event UnityAction UpdateEvent;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);//场景切换时不移除该对象
    }

    void Update()
    {
        UpdateEvent?.Invoke();
    }
    
    /// <summary>
    /// 添加事件到Update
    /// </summary>
    /// <param name="actionFunc"></param>
    public void AddEventListener(UnityAction actionFunc)
    {
        UpdateEvent += actionFunc;
    }
    
    /// <summary>
    /// 移除update中的指定事件
    /// </summary>
    /// <param name="actionFunc"></param>
    public void RemoveEventListener(UnityAction actionFunc)
    {
        UpdateEvent -= actionFunc;
    }
    
    //关于协程，直接调用此实例化对象的协程，不需要额外封装
}
