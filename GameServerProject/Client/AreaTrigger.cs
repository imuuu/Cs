using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class AreaTriggerDrop
{
    public GameObject _cardGameObject;
    public Card _card;
    public Guid _uuidAreaTrigger;
    public int _holderID = -1;
    public AreaTriggerDrop(Guid _triggerID,GameObject cardGameObject, Card card)
    {
        _cardGameObject = cardGameObject;
        _card = card;
        _uuidAreaTrigger = _triggerID;
    }
    public AreaTriggerDrop SetHolderID(int id)
    {
        _holderID = id;
        return this;
    }
}

public class AreaTriggerEnter
{
    public Card _begingDragCard;
    public Guid _uuidAreaTrigger;

    public AreaTriggerEnter(Guid _triggerID, Card begingDragCard)
    {
        _begingDragCard = begingDragCard;
        _uuidAreaTrigger = _triggerID;
    }
}

public class AreaTrigger : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Guid _uuid;
    private GameObject _onHoldDropObject;    
    protected GameManager _gameManager;
    protected Hand _hand;
    protected BuffCardManager _buffManager;
    private void Awake()
    {
        _uuid = Guid.NewGuid();
               
    }
    private void Start()
    {
        _gameManager = GameManager._instance;
        
        _hand = _gameManager.GetHand();
        _buffManager = _gameManager.GetBuffCardManager();
    }
    public Guid GetUUID()
    {
        return _uuid;
    }

    public void Register(Guid newUUID)
    {
        _uuid = newUUID;
        _gameManager.RegisterAreaTrigger(this);
        //Debug.Log("register: " + newUUID);
    }

    public void DestroyOnHoldDropObject(bool dest)
    {
        if(!dest)
        {
            _onHoldDropObject = null;
            return;
        }

        if (_onHoldDropObject == null)
            return;
        //Debug.Log("found hold card");

        CardUI cardUI = _onHoldDropObject.GetComponent<CardUI>();
        //if (cardUI != null)
        //{
        //    //Debug.Log("Removing from hand!");
        //    cardUI.GetCard().GetOwner().removeCardFromHand(_onHoldDropObject);
        //}
            
        Destroy(_onHoldDropObject);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject o = eventData.pointerDrag;
            CardUI cardUI = o.GetComponent<CardUI>();
            if (cardUI == null)
                return;

            Card card = cardUI.GetCard();
            if (card.GetState() != CardState.HAND)
                return;

            AreaTriggerDrop atd = new AreaTriggerDrop(_uuid,o, card);
            _onHoldDropObject = o;          
            OnCardDrop(atd);
            Events.onAreaTriggerDrop.Invoke(atd);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject cardGameobject = _hand.GetBeingDraggedCard();
        if (cardGameobject == null)
            return;
        AreaTriggerEnter ate = new AreaTriggerEnter(_uuid,cardGameobject.GetComponent<CardUI>().GetCard());
        OnCardEnter(ate);
        Events.onAreaTriggerEnter.Invoke(ate);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_buffManager.GetAskingHolder() > -1)
        {
            _buffManager.ResetIndicators();
        }
    }

    public virtual void OnCardDrop(AreaTriggerDrop areaTriggerDrop)
    {

    }

    public virtual void OnCardEnter(AreaTriggerEnter areaTriggerEnter)
    {

    }

}