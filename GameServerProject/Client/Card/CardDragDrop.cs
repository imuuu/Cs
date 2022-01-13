using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    RectTransform _rectTransform;
    Canvas _canvas;
    CanvasGroup _canvasGroup;

    TableCardHolder _tableCardHolder;
    TableCardHolder _lastTableCardHolder;

    Vector2 _lastRectAnchorPoint;
    bool _returnLastPoint = false;
    bool _drag_enabled = false;
    float _speedReturn = 1500;
    GameManager _gameManager;
    private void Awake()
    {
        _gameManager = GameManager._instance;
        _rectTransform = GetComponent<RectTransform>();
        print("on awake!: rect: " + _rectTransform);
    }

    private void Start()
    {      
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
        
    }
    //public void SetGameManager(GameManager gameManager)
    //{
    //    _gameManager = gameManager;
    //}
    private void FixedUpdate()
    {

        //can be made as async, no need to be in update all the time
        if (_returnLastPoint && !_drag_enabled && _lastRectAnchorPoint != null)
        {
            _rectTransform.position = Vector2.MoveTowards(_rectTransform.position, _lastRectAnchorPoint , Time.deltaTime * _speedReturn);
            if (Vector2.Distance(_rectTransform.position, _lastRectAnchorPoint) < 0.1)
            {
                setReturnLastPoint(false);
                if (_lastTableCardHolder != null && Vector2.Distance(_rectTransform.position, _lastTableCardHolder.GetComponent<RectTransform>().position) < 0.2)
                {
                    setTableHolder(_lastTableCardHolder);
                    disableTableHolder();
                }
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetComponent<CardUI>().GetCard()._ownerID != Client._instance._myId)
            return;

        if (_gameManager.GetBuffCardManager().isIndicatorsEnabled() && _gameManager.GetBuffCardManager().GetAskingHolder() < 0)
            return;

        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
        _returnLastPoint = true;
        _drag_enabled = true;
        _gameManager.GetHand().SetBeingDraggedCard(this.gameObject);
        if (_tableCardHolder != null && this.GetComponent<CardUI>().GetCard().GetState() == CardState.HAND)
            enableTableHolder();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _drag_enabled = false;
        _gameManager.GetHand().SetBeingDraggedCard(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuffCardManager buffManager = _gameManager.GetBuffCardManager();
        if (_gameManager.GetBuffCardManager().isIndicatorsEnabled() && _gameManager.GetBuffCardManager().GetAskingHolder() < 0)
        {
            //Card c = buffManager.GetCardFromIndicators(GetComponent<CardUI>().GetCard());
            int id = buffManager.SelectCardFromIndicators(GetComponent<CardUI>().GetCard());
            if (id > -1)
            {
                List<Guid> selectedCards = buffManager.GetAllSelectedCardUUIDs();
                if(buffManager.GetTotalChoses() <= selectedCards.Count)
                {
                    ClientSend.SendChosenIndicatorCard(selectedCards);
                    return;
                }
                //Debug.Log("Not enough choses: "+ selectedCards.Count);
                return;
            }
        }
        //Debug.Log("onPointerDown");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        if (_gameManager.GetBuffCardManager().isIndicatorsEnabled() && _gameManager.GetBuffCardManager().GetAskingHolder() < 0)
            return;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        //Debug.Log(eventData.);
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void setTableHolder(TableCardHolder tcb)
    {
        _tableCardHolder = tcb;
        _lastTableCardHolder = tcb;
    }

    public void enableTableHolder()
    {
        //_tableCardHolder.gameObject.SetActive(true);
        _tableCardHolder.SetDropEnable(true);
        _tableCardHolder = null;
    }

    public void disableTableHolder()
    {
        //_tableCardHolder.gameObject.SetActive(false);
        _tableCardHolder.SetDropEnable(false);
    }



    public void setLastRectAnchorPoint(Vector2 pos)
    {
        _lastRectAnchorPoint = pos;
    }
    public void setLastRectAnchorPoint()
    {
        print("rect: " + _rectTransform);
        _lastRectAnchorPoint = _rectTransform.position;
    }
    public Vector2 GetLastRectAnchorPoint()
    {
        return _lastRectAnchorPoint;
    }
    public void setReturnLastPoint(bool b)
    {
        _returnLastPoint = b;
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
            Card victim = GetComponent<CardUI>().GetCard();
            Guid attTriggerUUID = Guid.Empty;
            if(o.GetComponent<CardDragDrop>()._tableCardHolder != null)
            {
                if(o.GetComponent<TableCardHolder>() != null)
                    attTriggerUUID = o.GetComponent<TableCardHolder>().GetUUID();
            }
               
            
            Guid vicTriggerUUID = Guid.Empty;
            
            if (_tableCardHolder != null)
                vicTriggerUUID = _tableCardHolder.GetComponent<TableCardHolder>().GetUUID();

            if (card.GetState() == CardState.FRONTLINE || card.GetState() == CardState.BACKLINE)
            {
                
                ClientSend.SendBattleBetweenCards(attTriggerUUID, vicTriggerUUID, card._uuid, victim._uuid);
                o.GetComponent<RectTransform>().position = o.GetComponent<CardDragDrop>().GetLastRectAnchorPoint();
            }else if(card._classType == CardClassType.Spell)
            {
                ClientSend.SendBattleBetweenCards(attTriggerUUID, vicTriggerUUID, card._uuid, victim._uuid);
            }
            else
            {
                Debug.Log("card isnt battle field");
            }
        }
    }
}
