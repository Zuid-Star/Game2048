using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> where T : new()         //约束T必须包含一个公共无参构造函数
{
    private static T _instance;

    /// <summary>
    /// 获取单例类
    /// </summary>
    /// <returns></returns>
    public static T GetInstance()
    { 
        return _instance ?? (_instance = new T());    // ？？是空值合并运算符，（）内的内容是执行内容，如果_instance为空的话，（）的内容会执行 
    }
}
