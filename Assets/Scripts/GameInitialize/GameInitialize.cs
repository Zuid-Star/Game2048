using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialize : MonoBehaviour
{
    //这个类负责游戏初始化一些参数
    void Start()
    {
        UiManager.GetInstance().ShowPanel("GameUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
