using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CardBase : MonoBehaviour
{
    private int power = 1;//指数
    public int Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
        }
    }
    
    private int value = 2;//具体数值
    public int Value => value;
    
    private TextMeshPro _tmp;
    
    private SpriteRenderer _spriteRenderer;

    private ColorType _colorType = ColorType.A;
    
    private void Awake()
    {
        _tmp = GetComponentInChildren<TextMeshPro>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChangePower(1);
    }

    private void OnEnable()
    {
        NewCard();
    }

    private void OnDisable()
    {
        DestroyCard();
    }

    //改变数值
    private void ChangePower(int power)
    {
        this.Power = power;
        value = (int)Mathf.Pow(2, power);
        ChangeColor();
        if (_tmp != null)
        {
            _tmp.text = value.ToString();
        }

        //如果大于全局最大pow，则更新全局最大pow,最大不超过10
        if (Power > DataManager.GetInstance().MaxPow && Power <= 10)
        {
            DataManager.GetInstance().MaxPow = Power;
        }
    }
    
    //改变卡牌颜色
    private void ChangeColor()
    {
        _colorType = (ColorType)Power;
        switch (_colorType)
        {
            case ColorType.A:
                _spriteRenderer.color = Color.white;
                break;
            case ColorType.B:
                _spriteRenderer.color = new Color32(241, 119, 22,255);
                break;
            case ColorType.C:
                _spriteRenderer.color = new Color32(190, 70, 181,255);
                break;
            case ColorType.D:
                _spriteRenderer.color = new Color32(114, 109, 138,255);
                break;
            case ColorType.E:
                _spriteRenderer.color = new Color32(60, 176, 218,255);
                break;
            case ColorType.F:
                _spriteRenderer.color = new Color32(113, 186, 26,255);
                break;
            case ColorType.G:
                _spriteRenderer.color = new Color32(238, 144, 173,255);
                break;
            case ColorType.H:
                _spriteRenderer.color = new Color32(142, 143, 135,255);
                break;
            case ColorType.I:
                _spriteRenderer.color = new Color32(21, 138, 145,255);
                break;
            case ColorType.J:
                _spriteRenderer.color = new Color32(94, 169, 25,255);
                break;
            case ColorType.K:
                _spriteRenderer.color = new Color32(249, 198, 41,255);
                break;
            case ColorType.L:
                _spriteRenderer.color = Color.red;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //指数增加（数量翻倍）
    public void AddPower()
    {
        ChangePower(Power + 1);
    }

    //新卡生成新数值
    private void NewCard()
    {
        int maxPow = DataManager.GetInstance().MaxPow + 1;
        int pow = Random.Range(1, maxPow);
        ChangePower(pow);
    }
    
    //跟随事件
    public void FallowAction(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }
    
    //对象检测事件,与跟随事件触发条件相同，所以直接假如到跟随事件中
    public void SelectAction(InputAction.CallbackContext context)
    {
        Vector2 pos = InputManager.GetInstance().inputMaps.TouchScreen.PointerPosition.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(pos);
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
                
            }
            else if(miniDisHit.name == "CardDis")
            {
                
            }
        }
    }
    
    public void DestroyCard()
    {
        // PoolManager.GetInstance().ReturnObject("CardBase",gameObject);
    }
    
    
}
