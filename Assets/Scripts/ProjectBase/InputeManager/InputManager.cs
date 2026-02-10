using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBase<InputManager>
{
    public InputMaps inputMaps;

    private CardBase currentCard;
    
    private void Awake()
    {
        inputMaps = new InputMaps();
    }

    private void Start()
    {
        inputMaps.TouchScreen.PointerPress.started += PressScreen;
        inputMaps.TouchScreen.PointerPress.canceled += ReleaseScreen;
    }

    private void OnEnable()
    {
        inputMaps.Enable();
    }

    private void OnDisable()
    {
        inputMaps.Disable();
    }

    private void OnDestroy()
    {
        inputMaps.Dispose();
    }
    
    
    //点击屏幕触发事件
    private void PressScreen(InputAction.CallbackContext context)
    {
        Vector2 pos = inputMaps.TouchScreen.PointerPosition.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(pos);
        //射线检测
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.name == "CreateCard")
            {
                currentCard = hit.collider.GetComponent<CreateCardList>().GetCard;
                inputMaps.TouchScreen.PointerPosition.performed += currentCard.FallowAction;
                currentCard.gameObject.transform.position += Vector3.back;
                
                currentCard.gameObject.GetComponent<BoxCollider2D>().enabled = true;//打开collider，方便进行选中对象的检测
                
                AudioManager.GetInstance().PlaySound("CardWave");//播放音效
            }
        }
    }
    
    //松开屏幕触发事件
    private void ReleaseScreen(InputAction.CallbackContext context)
    {
        Vector2 pos = inputMaps.TouchScreen.PointerPosition.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(pos);
        
        if (currentCard != null)
        {
            currentCard.gameObject.GetComponent<BoxCollider2D>().enabled = false;//关闭collider，防止干扰
            //范围检测
            Collider2D[] hits = Physics2D.OverlapBoxAll(worldPos, new Vector2(1,1.5f), 0,LayerMask.GetMask("CardSlot"));
            if (hits.Length > 0)
            {
                
                Collider2D miniDisHit = hits[0];
                foreach (Collider2D hit in hits)
                {
                    float dist = Vector2.Distance(hit.transform.position, worldPos);
                    float currentDist = Vector2.Distance(miniDisHit.transform.position, worldPos);
                    if (dist < currentDist)
                    {
                        miniDisHit = hit;
                    }
                }

                if (miniDisHit.name == "CardList")
                {
                    miniDisHit.gameObject.GetComponent<CardList>().CardListAdd(currentCard);
                }
                else if(miniDisHit.name == "CardDis")
                {
                    EventManager.GetInstance().EventTrigger("CardDis",currentCard);
                }
            }
            else
            {
                DataManager.GetInstance().createCardList.CardBack();
            }
            
            inputMaps.TouchScreen.PointerPosition.performed -= currentCard.FallowAction;
            currentCard = null;
            AudioManager.GetInstance().PlaySound("CardWave");
        }
    }
}
