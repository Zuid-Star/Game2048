using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour  //将T约束为Mono behaviour
{
    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            _instance = obj.AddComponent<T>();
        }//通过创建game object的方式间接创建instance实例，不需要将此脚本挂载到场景对象上即可使用，避免了挂载多个脚本导致的覆盖问题
        return _instance;
    }
}
