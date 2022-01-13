using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;
using System;

public class CollectionCardPlate : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]TextMeshProUGUI _textClickedAmount;
    int _clickedAmount = 0;
    GameObject _cardGameobject;
    Action<int,int> _clickAmountAction;
    int _slot = -1;
    private void Awake()
    {
        _textClickedAmount.text = "";
       
    }

    public void SetClickAction(Action<int, int> action)
    {
        _clickAmountAction = action;
    }

    public int GetclickedAmount()
    {
        return _clickedAmount;
    }

    public void SetClickAmount(int amount)
    {
        _clickedAmount = amount;     
        if (_clickedAmount <= 0)
        {
            _clickedAmount = 0;
            _textClickedAmount.text = "";
            return;
        }

        _textClickedAmount.text = "x " + _clickedAmount;
    }

    public void ChangeCard(GameObject gameObject,int slot , int dir, int clickAmount)
    {
        _slot = slot;
        gameObject.transform.SetParent(this.transform);
        gameObject.transform.localPosition = new Vector3(Screen.width * dir, 0, 0);
        gameObject.transform.localScale *= 2.68f; // 

        GameObject obj = _cardGameobject;
        if(obj != null)
        {
            SetClickAmount(0);
            obj.transform.LeanMoveLocalX(-Screen.width * dir, 0.5f).setEaseInExpo().setOnComplete(c =>
            {
                Destroy(obj);
            });
        }

        _cardGameobject = gameObject;
        _cardGameobject.transform.LeanMoveLocalX(0, 0.5f).setEaseInExpo().setOnComplete(() => SetClickAmount(clickAmount));

    }

    void IncreaseClickeAmount(int amount)
    {
        //print("amount: " + amount);
        _clickedAmount += amount;
        if (_clickedAmount <= 0) _clickedAmount = 0;
    
        if (_slot != -1 && _clickAmountAction != null)
            _clickAmountAction(_slot, _clickedAmount);
        
        SetClickAmount(_clickedAmount);

        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        print("event data");
        switch (eventData.button)
       {
            case PointerEventData.InputButton.Left:
                IncreaseClickeAmount(1);
                break;
            case PointerEventData.InputButton.Right:
                IncreaseClickeAmount(-1);
                break;
        }
    }
}
