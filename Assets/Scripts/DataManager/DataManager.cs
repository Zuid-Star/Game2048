using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSave
{
    public int bestScore;
}

public class DataManager : SingletonMonoBase<DataManager>
{
    private int maxPow = 2;//当前最大指数
    public int MaxPow
    {
        get
        {
            return maxPow;
        }
        set
        {
            maxPow = value;
        }
    }
    private DataSave dataSave;
    
    private int score = 0;//分数
    private int bestScore = 0;//最大分数
    public int Score => score;
    public int BestScore => bestScore;

    public CreateCardList createCardList;
    public List<CardList> cardListList;
    public CardDis cardDis;

    public DataManager()
    {
        cardListList  = new List<CardList>();
    }
    
    private void Start()
    {
        EventManager.GetInstance().AddEventListener<CardBase>("AddScore",AddScoreOnDataManager);
        ReadData();
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveEventListener<CardBase>("AddScore",AddScoreOnDataManager);
    }


    //分数改变时触发
    private void AddScoreOnDataManager(CardBase card)
    {
        score += card.Value;
    }

    //检查游戏是否结束
    public void GameOverCheck()
    {
        //结束条件
        if (cardDis.DisNumber == 0)
        {
            foreach (CardList cardList in cardListList)
            {
                if (cardList.CardListLong < 8 || cardList.LastPow == createCardList.CurrentPow)
                {
                    return;
                }
            }
            //符合条件的话就触发game over事件
            EventManager.GetInstance().EventTrigger("GameOver", score);
            CompareScore();
            UiManager.GetInstance().ShowPanel("GameOverUI");
            InputManager.GetInstance().inputMaps.Disable();
        }
    }
    
    
    //游戏结束时需要比较当前分数与最大分数，并更新
    private void CompareScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            dataSave.bestScore = bestScore;
            
            string jsonString = JsonUtility.ToJson(dataSave);
            File.WriteAllText(Application.persistentDataPath + "/Game2048Data.json", jsonString);//存储到本地文件
        }
    }
    
    //读取数据
    private void ReadData()
    {
        dataSave = new DataSave(){bestScore = 0};
        if (File.Exists(Application.persistentDataPath + "/Game2048Data.json"))
        {
            string jsonString = File.ReadAllText(Application.persistentDataPath + "/Game2048Data.json");//读取
            dataSave = JsonUtility.FromJson<DataSave>(jsonString);
            bestScore = dataSave.bestScore;
        }
    }
}
