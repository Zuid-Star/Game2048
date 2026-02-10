using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitialize : MonoBehaviour
{
    
    void Start()
    {
        UiManager.GetInstance().ShowPanel("MainMenu");
    }
    
}
