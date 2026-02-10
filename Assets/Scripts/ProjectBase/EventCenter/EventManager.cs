using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


/// 通过在外部用接口和类封装一个泛型UnityAction<T>,实现事件中心里面字典中泛型的需求，避免了用object参数的泛型事件需要频繁 类型转换的缺点
/// 由于父接口并非泛型，所以可以使用父接口来存储泛型子类
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public interface IEventInfo
{
    
}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> Actions;
    //这个构造函数是为了在创建事件监听时 直接将第一个事件加入
    public EventInfo(UnityAction<T> actionFunc)
    {
        Actions += actionFunc;
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

public class EventManager : SingletonBase<EventManager>
{
    //通过字典存储监听的事件
    private Dictionary<string, IEventInfo> _eventsDictionary = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件函数到监听器中
    /// </summary>
    /// <param name="eventName">目标事件监听器</param>
    /// <param name="actionFunc">添加的事件</param>
    /// <typeparam name="T">添加事件的参数类型</typeparam>
    public void AddEventListener<T>(string eventName, UnityAction<T> actionFunc)
    {
        if (_eventsDictionary.ContainsKey(eventName))
        {
            (_eventsDictionary[eventName] as EventInfo<T>).Actions += actionFunc;
        }
        else
        {
            _eventsDictionary.Add(eventName, new EventInfo<T>(actionFunc));
        }
    }
    
    /// <summary>
    /// 移除监听器中的事件函数
    /// </summary>
    /// <param name="eventName">目标事件监听器</param>
    /// <param name="actionFunc">移除的事件</param>
    /// <typeparam name="T">移除事件的参数类型</typeparam>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> actionFunc)
    {
        if (_eventsDictionary.ContainsKey(eventName))
        {
            (_eventsDictionary[eventName] as EventInfo<T>).Actions -= actionFunc;
        }    
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="eventName">事件监听器名称</param>
    /// <param name="info">事件触发传入的参数</param>
    /// <typeparam name="T">事件触发传入参数的类型</typeparam>
    public void EventTrigger<T>(string eventName,T info)
    {
        if (_eventsDictionary.ContainsKey(eventName))
        {
            (_eventsDictionary[eventName] as EventInfo<T>).Actions?.Invoke(info);//如果没有相应事件，不用处理(表示没有需要触发的事件)
        }
    }
    
    /// <summary>
    /// 清空所有事件
    /// </summary>
    public void Clear()
    {
        _eventsDictionary.Clear();
    }
}
