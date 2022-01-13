using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CardBuilder _cardBuilder;
    PlayerManager _pManager;
    [SerializeField] private Hand _handScript;
    [SerializeField] private GameObject[] _handCards;
    [SerializeField] int _handSize; // set by server
    int _totalCardsInHand = 0;
    GameManager _gameManager;
    private void Awake()
    {
         SetHandSize(_handSize);
        _handScript = FindObjectOfType<Hand>();
        _cardBuilder = FindObjectOfType<CardBuilder>();
        _gameManager = FindObjectOfType<GameManager>();
        _pManager = GetComponent<PlayerManager>();
    }

    public void SetHandSize(int size)
    {
        _handSize = size;
        _handCards = new GameObject[_handSize];
    }
    public bool AddCardToHand(Card card)
    {
        if (isHandFull())
            return false;

        for(int i = 0; i < _handSize; ++i)
        {
            if(_handCards[i] == null)
            {
                card.SetOwner(this);
                _handCards[i] = _cardBuilder.SpawnCard(card,_gameManager);
                _totalCardsInHand++;
                if (_totalCardsInHand > _handSize)
                    _totalCardsInHand = _handSize;
                
                UpdateHandUI();
                return true;
            }
        }
        return false;
    }
    public void UpdateHandUI()
    {
        _handScript.UpdateHandUI(_handCards, _totalCardsInHand);
    }
    public GameObject removeCardFromHand(Guid card_uuid)
    {

        for (int i = 0; i < _handCards.Length; ++i)
        {
            Debug.Log("=>..."+ _handCards[i]);
            if (_handCards[i] == null)
                continue;

            CardUI cUI = _handCards[i].GetComponent<CardUI>();
            if(cUI == null)
                continue;

            if (cUI.GetCard()._uuid.Equals(card_uuid))
            {
                GameObject cardO = _handCards[i];
                _handCards[i] = null;
                _totalCardsInHand--;
                if (_totalCardsInHand < 0)
                    _totalCardsInHand = 0;

                UpdateHandUI();
                return cardO;
            }
        }
        return null;
    }
    public GameObject removeCardFromHand(GameObject card)
    {
        CardUI cUI = card.GetComponent<CardUI>();
        if (cUI == null)
            return null;

        return removeCardFromHand(cUI.GetCard()._uuid);
    }

    public bool isHandFull()
    {
        return _totalCardsInHand >= _handSize;
    }

    public GameObject getCard(int index)
    {
        return _handCards[index];
    }

    public PlayerManager GetPlayerManager()
    {
        return _pManager;
    }
}
