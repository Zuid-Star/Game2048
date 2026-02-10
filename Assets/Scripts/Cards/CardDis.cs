using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDis : MonoBehaviour
{
    private int disNumber = 2;
    public int DisNumber => disNumber;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        DataManager.GetInstance().cardDis = this;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        EventManager.GetInstance().AddEventListener<CardBase>("CardDis",CardDisAction);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveEventListener<CardBase>("CardDis",CardDisAction);
    }

    //删除卡牌
    private void DisCard(GameObject cardObj)
    {
        if (disNumber > 0)
        {
            disNumber--;
            PoolManager.GetInstance().ReturnObject("CardBase",cardObj);
        }
    }

    //将卡牌放到垃圾堆时触发事件
    public void CardDisAction(CardBase card)
    {
        CreateCardList createCardList = DataManager.GetInstance().createCardList;
        //判断垃圾桶是否还能装下
        if (DisNumber > 0)
        {
            createCardList.RemoveCard();
            StartCoroutine(CardBackCoroutine(card));
        }
        else
        {
            createCardList.CardBack();
        }
    }
    
    //卡牌进入垃圾堆，协程动画
    IEnumerator CardBackCoroutine(CardBase card)
    {
        Vector3 targetPos = transform.position + Vector3.back;
        while (true)
        {
            card.gameObject.transform.position = Vector3.Lerp(card.gameObject.transform.position, targetPos, Time.fixedDeltaTime * 15);
            float distance = Vector3.Distance(card.gameObject.transform.position, targetPos);
            if (distance < 0.05f)
            {
                card.gameObject.transform.position =  targetPos;
                DisCard(card.gameObject);
                ChangeTexture();
                
                //卡牌进入垃圾堆动画结束后进行一次game over检测
                DataManager.GetInstance().GameOverCheck();
                
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    //垃圾堆切换贴图
    private void ChangeTexture()
    {
        switch (DisNumber)
        {
            case 0:
                ResourceManager.GetInstance().LoadAsync("Texture/2048Card_5", (Sprite sprite) =>
                {
                    _spriteRenderer.sprite = sprite;
                });
                break;
            case 1:
                ResourceManager.GetInstance().LoadAsync("Texture/2048Card_4", (Sprite sprite) =>
                {
                    _spriteRenderer.sprite = sprite;
                });
                break;
            case 2:
                ResourceManager.GetInstance().LoadAsync("Texture/2048Card_2", (Sprite sprite) =>
                {
                    _spriteRenderer.sprite = sprite;
                });
                break;
        }
    }
}
