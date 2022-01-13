using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableCardHolder : TableAreaTrigger
{
    [SerializeField] int _id = -1;
    [SerializeField] CardHolderType _type;
    [SerializeField] GameObject _card;
    [SerializeField] GameObject _enabledGameobject;
    [SerializeField] bool _enable_drop = true;
  
    
    public override void OnCardDrop(AreaTriggerDrop areaTriggerDrop)
    {
        //if (_type == CardHolderType.MIRROR_FRONTLINE || _type == CardHolderType.MIRROR_BACKLINE)
        //    return;

        //ClientSend.DeckDropCardToHolder(this, areaTriggerDrop._card);
        print("on card drop");
        ClientSend.OnAreaTableTriggerDrop(areaTriggerDrop.SetHolderID(GetId()));
    }
    /// <summary>
    /// Card has put here when dropped to holder and waiting for server anwser to destroy it
    /// </summary>
    //public void SetCardToReadyToDestroy(GameObject cardGameobject)
    //{
    //    _cardReadyToDestroy = cardGameobject;
    //}

    //public void DestroyReadyCard()
    //{
    //    _cardReadyToDestroy.GetComponent<CardUI>().GetCard().GetOwner().removeCardFromHand(_cardReadyToDestroy);
    //    Destroy(_cardReadyToDestroy);
    //}
    public GameObject GetCardGameobject()
    {
        return _card;
    }

    public Card GetCard()
    {
        if (_card == null)
            return null;

        return _card.GetComponent<CardUI>().GetCard();
    }

    public int GetId()
    {
        return _id;
    }

    public CardHolderType GetHolderType()
    {
        return _type;
    }

    public void SetCard(GameObject o)
    {     
        
        o.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        o.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());
        o.GetComponent<CardDragDrop>().setTableHolder(this);
        SetDropEnable(false);
        o.GetComponent<CardDragDrop>().setLastRectAnchorPoint();
        o.GetComponent<CardDragDrop>().setReturnLastPoint(false);

        _card = o;
    }

    public void UpdateCard(Card card)
    {
        GameObject o = GetCardGameobject();
        if (o == null)
        {
            return;
        }
        o.GetComponent<CardUI>().SetCardUIData(card);
        
    }

    public void SetDropEnable(bool b)
    {
        _enable_drop = b;
        if(b)
        {
            _enabledGameobject.SetActive(true);
            return;
        }
        _enabledGameobject.SetActive(false);
    }

    public override void OnCardEnter(AreaTriggerEnter areaTriggerEnter)
    {
        _buffManager.SetAskingHolder(GetId());
        ClientSend.OnEnterCardHolder(GetId(), areaTriggerEnter._begingDragCard._uuid, CardHolderEnterType.SHOW_INDICATORS);
    }
   
}
