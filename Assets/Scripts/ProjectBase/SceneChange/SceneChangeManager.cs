using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonBase<SceneChangeManager>
{
    /// <summary>
    /// 切换场景 同步
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="callback">加载完成后回调函数</param>
    public void LoadScene(string sceneName,UnityAction callback)
    {
        UiManager.GetInstance().ClearPanels();//清空ui管理器中的ui
        PoolManager.GetInstance().ClearPool();//清空对象池
        //场景同步加载 
        SceneManager.LoadScene(sceneName);
        callback?.Invoke();
    }

    /// <summary>
    /// 场景异步加载
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="callback">回调函数</param>
    public void LoadSceneAsync(string sceneName, UnityAction callback)
    {
        //使用公共mono类启动协程
        MonoManager.GetInstance().StartCoroutine(ReallyLoadSceneAsync(sceneName, callback));
    }

    /// <summary>
    /// 场景异步加载协程函数
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="callback">回调函数</param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsync(string sceneName, UnityAction callback)
    {
        UiManager.GetInstance().ClearPanels();//清空ui管理器中的ui
        PoolManager.GetInstance().ClearPool();//清空对象池
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);//AsyncOperation包含了一些场景加载相关数据，如加载进度、是否加载完毕等
        
        while (!asyncOperation.isDone)
        {
            //通过事件中心将加载进度向外分发,asyncOperation.progress是加载进度,外部可以通过监听"LoadSceneProgress"来获取 加载进度 并执行一些方法
            EventManager.GetInstance().EventTrigger("LoadSceneProgress", asyncOperation.progress);
            
            yield return null;
        }
        
        yield return asyncOperation;
        //加载完成后执行回调函数
        callback();
    }
}
