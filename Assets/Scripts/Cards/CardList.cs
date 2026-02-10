using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    private List<CardBase> cardList;
    
    private BoxCollider2D boxCollider;
    
    //当前卡组长度
    public int CardListLong => cardList.Count;
    
    //末端卡牌指数
    public int LastPow => cardList[CardListLong -1].Power;

    private void Awake()
    {
        cardList = new List<CardBase>();
        boxCollider = GetComponent<BoxCollider2D>();
        DataManager.GetInstance().cardListList.Add(this);
    }

    void Start()
    {

    }
    
    //添加卡牌
    private void AddCard(CardBase card)
    {
        if (CardListLong >= 1) 
        {
            cardList.Add(card);
            boxCollider.offset += Vector2.down * 0.5f;
            card.gameObject.transform.SetParent(transform);
                
            Vector3 targetPosition = cardList[CardListLong - 2].transform.localPosition + new Vector3(0f, -0.5f, -1f);
            StartCoroutine(AddCardCoroutine(card, targetPosition));
        }
        else
        {
            cardList.Add(card);
            card.gameObject.transform.SetParent(transform);
            
            StartCoroutine(AddCardCoroutine(card, Vector3.back));
        }
    }

    //添加卡牌到列表时的协程，动画
    IEnumerator AddCardCoroutine(CardBase card,Vector3 targetPosition)
    {
        while (true)
        {
            card.gameObject.transform.localPosition = Vector3.Lerp(card.gameObject.transform.localPosition, targetPosition, Time.fixedDeltaTime * 15);
            float distance = Vector3.Distance(card.gameObject.transform.localPosition, targetPosition);
            if (distance < 0.05f)
            {
                card.gameObject.transform.localPosition =  targetPosition;
                CompareValue();//动画完成后再进行数值比较
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    //分数检测
    private void CompareValue()
    {
        if (CardListLong >= 2) 
        {
            if (cardList[CardListLong - 1].Power == cardList[CardListLong - 2].Power)
            {
                StartCoroutine(CompareValueCoroutine());
                AudioManager.GetInstance().PlaySound("CardWave");
            }
            else if (CardListLong == 8)//当卡组长度等于8时（即满卡状态），触发一次game over检测
            {
                DataManager.GetInstance().GameOverCheck();
            }
            
        }
    }

    //比较完成后，卡牌向上移动动画协程
    IEnumerator CompareValueCoroutine()
    {
        Vector3 targetPosition = cardList[CardListLong - 2].gameObject.transform.localPosition + Vector3.back;
        while (true)
        {
            cardList[CardListLong - 1].gameObject.transform.localPosition = Vector3.Lerp(cardList[CardListLong - 1].gameObject.transform.localPosition, targetPosition, Time.fixedDeltaTime * 10);
            float distance = Vector3.Distance(cardList[CardListLong - 1].gameObject.transform.localPosition, targetPosition);
            if (distance < 0.05f)
            {
                cardList[CardListLong - 1].gameObject.transform.localPosition =  targetPosition;
                yield return new WaitForFixedUpdate();//等待一帧确保动画完成
                //动画完成后再进行逻辑判断
                cardList[CardListLong - 2].AddPower();
                if (cardList[CardListLong - 2].Power == 11)
                {
                    RemoveCard();
                }
                RemoveCard();
                
                //递归检测
                CompareValue();
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    //移除卡牌,只会移除列表最后一位
    private void RemoveCard()
    {
        CardBase card = cardList[CardListLong - 1];
        EventManager.GetInstance().EventTrigger("AddScore",card);
        cardList.RemoveAt(CardListLong - 1);
        boxCollider.offset -= Vector2.down * 0.5f;
        PoolManager.GetInstance().ReturnObject("CardBase",card.gameObject);
    }

    //卡牌移动到卡槽时触发的事件(提供给外部调用)
    public void CardListAdd(CardBase card)
    {
        CreateCardList createCardList = DataManager.GetInstance().createCardList;
        if (CardListLong < 8)
        {
            AddCard(card);
            createCardList.RemoveCard();
        }
        else if(CardListLong == 8)
        {
            if (cardList[CardListLong - 1].Power == card.Power)
            {
                AddCard(card);
                createCardList.RemoveCard();
            }
            else
            {
                createCardList.CardBack();
            }
        }
        else
        {
            createCardList.CardBack();
        }
    }
    
}
