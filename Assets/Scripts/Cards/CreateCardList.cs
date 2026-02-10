using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreateCardList : MonoBehaviour
{
    public float cardMoveSpeed = 15;
    
    private List<CardBase> _cardList;

    public CardBase GetCard
    {
        get{return _cardList[0];}
    }
    
    public int CurrentPow => _cardList[0].Power;
    
    private InputManager inputManager;
    
    private void Awake()
    {
        _cardList = new List<CardBase>();
        inputManager = InputManager.GetInstance();
        DataManager.GetInstance().createCardList = this;
    }

    void Start()
    {
        CreateCard();
        CreateCard();
    }
    

    //创建卡牌
    void CreateCard()
    {
        PoolManager.GetInstance().GetObject("CardBase","Prafab/CardBase", (obj) =>
        {
            CardBase card = obj.GetComponent<CardBase>();
            obj.transform.SetParent(transform);
            _cardList.Add(card);
            card.transform.localPosition = Vector3.back;
            if (_cardList.Count > 1)
            {
                _cardList[0].gameObject.transform.localPosition += Vector3.back;
                StartCoroutine(CreateCardCoroutine());
            }
        });
    }
    
    //卡牌生成协程，过度动画
    IEnumerator CreateCardCoroutine()
    {
        Vector3 targetPos = Vector3.right + Vector3.back;
        while (true)
        {
            _cardList[0].gameObject.transform.localPosition = Vector3.Lerp(_cardList[0].gameObject.transform.localPosition, targetPos, Time.fixedDeltaTime * cardMoveSpeed);
            float distance = Vector3.Distance(_cardList[0].gameObject.transform.localPosition, targetPos);
            if (distance < 0.05f)
            {
                _cardList[0].gameObject.transform.localPosition =  targetPos;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    //取出卡牌
    public void RemoveCard()
    {
        _cardList.RemoveAt(0);
        CreateCard();
    }
    
    //卡牌返回
    public void CardBack()
    {
        StartCoroutine(CardBackCoroutine());
    }

    //卡牌返回协程，过度动画
    IEnumerator CardBackCoroutine()
    {
        Vector3 targetPos = new Vector3(1f, 0, -1f);
        while (true)
        {
            _cardList[0].gameObject.transform.localPosition = Vector3.Lerp(_cardList[0].gameObject.transform.localPosition, targetPos, Time.fixedDeltaTime * cardMoveSpeed);
            float distance = Vector3.Distance(_cardList[0].gameObject.transform.localPosition, targetPos);
            if (distance < 0.05f)
            {
                _cardList[0].gameObject.transform.localPosition =  targetPos;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
}
