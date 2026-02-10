using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象池管理器
/// 避免频繁创建于销毁对象造成性能开销
/// </summary>
public class PoolManager : SingletonBase<PoolManager>
{
    //缓存池容器，用字典存储不同的对象池，而具体对象池用queue
    private Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();

    private GameObject _poolObject;//一个场景上的对象，作为对象池中未启用对象的父类，便于直观管理对象池

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="objPath">预制体路径（如果对象池中没有对象，需要从外部加载）</param>
    /// <param name="callBack">获取对象后回调函数</param>
    public void GetObject(string poolName,string objPath, UnityAction<GameObject> callBack)
    {
        if (_pools.ContainsKey(poolName) && _pools[poolName].Count > 0)
        {
            GameObject obj = _pools[poolName].Dequeue();//对象池中存在对象，取出第一个对象并从queue中移除索引
            obj.SetActive(true);//激活对象,让其在场景中出现
            obj.transform.parent = null;//从pool场景对象中取出
            callBack(obj);
        }
        else
        {
            //使用异步加载，减少卡顿
            ResourceManager.GetInstance().LoadAsync<GameObject>(objPath, (gameObject) =>
            {
                gameObject.name = poolName;//修改名称与对象池一致，因为预制体实例化会有（clone）后缀
                gameObject.SetActive(true);//激活对象,让其在场景中出现 
                callBack(gameObject);//回调函数
            });
        }
    }

    /// <summary>
    /// 将对象返回对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="obj">待返回的对象</param>
    public void ReturnObject(string poolName, GameObject obj)
    {
        if (_poolObject == null)
        {
            _poolObject = new GameObject();
            _poolObject.name = "Pool";
        }
        obj.transform.parent = _poolObject.transform;//设置为pool子对象，便于观察与管理
        
        obj.SetActive(false);//让对象从场景中消失(只是失活，未删除)
        if (_pools.ContainsKey(poolName))
        {
            _pools[poolName].Enqueue(obj);//如果存在队列，就直接添加对象
        }
        else
        {
            _pools.Add(poolName, new Queue<GameObject>());
            _pools[poolName].Enqueue(obj);//如果没有队列就添加队列，然后在队列中添加对象
        }
    }
    
    /// <summary>
    /// 清空对象池
    /// </summary>
    /// <param name="poolName"></param>
    public void ClearPool()
    {
        _pools.Clear();
        _poolObject = null;
    }
}
