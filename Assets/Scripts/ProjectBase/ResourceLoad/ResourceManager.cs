using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载管理器
/// 包含 同步资源加载 和 异步资源加载
/// </summary>
public class ResourceManager : SingletonBase<ResourceManager>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="resourcePath">资源路径</param>
    /// <typeparam name="T">资源类型</typeparam>
    /// <returns></returns>
    public T Load<T>(string resourcePath) where T : Object
    {
        T resource = Resources.Load<T>(resourcePath);//返回值直接是T，不需要类型转换
        //如果对象是一个GameObject类型，直接实例化后再返回,外部可以直接使用
        if (resource is GameObject)
        {
            resource = GameObject.Instantiate(resource);
        }
        return resource;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="resourcePath">资源路径</param>
    /// <param name="callBack">加载完成后回到函数</param>
    /// <typeparam name="T">资源类型</typeparam>
    public void LoadAsync<T>(string resourcePath, UnityAction<T> callBack) where T : Object
    {
        //使用公共mono类开启协程异步加载
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync(resourcePath, callBack));
    }

    private IEnumerator ReallyLoadAsync<T>(string resourcePath, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<T>(resourcePath);//返回值是ResourceRequest，其中的resourceRequest.asset是真正的加载数据,类型为GameObject,需要类型转换
        yield return resourceRequest;//加载完成后才会进入接下来的步骤(调用回调函数callBack)

        //如果对象是一个GameObject类型，直接实例化后再返回,外部可以直接使用
        if (resourceRequest.asset is GameObject)
        {
            callBack(GameObject.Instantiate(resourceRequest.asset as T));
        }
        else//不需要实例化的对象可以直接返回
        {
            callBack(resourceRequest.asset as T);
        }
    }
}
