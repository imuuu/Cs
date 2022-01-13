using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffCardManager : MonoBehaviour
{
    private GameObject[] _cards_enabled_indicator = new GameObject[16]; //size is amount of 
    private bool[] _cards_enabled_indicator_chosen = new bool[16]; //size is amount of 
    
    private int _index = 0;
    private bool _indicators_enabled = false;
    private int _total_choses = 0;
    private Color _indicator_color = Color.white;
    private Color _select_indicator_color = Color.cyan;

    private int _asking_from_holder = -1;
    private void Awake()
    {
        //_cards_enabled_indicator = new GameObject[GameObject.FindObjectsOfType<TableCardHolder>().Length];    
    }

    public bool isIndicatorsEnabled()
    {
        return _indicators_enabled;
    }

    public void SetTotalChoses(int amount)
    {
        _total_choses = amount;
    }

    public int GetTotalChoses()
    {
        return _total_choses;
    }

    public void SetAskingHolder(int holderID)
    {
        _asking_from_holder = holderID;
    }

    public int GetAskingHolder()
    {
        return _asking_from_holder;
    }
    public void SetIndicatorColor(Color color)
    {
        _indicator_color = color;
    }
    public void SetSelectIndicatorColor(Color color)
    {
        _select_indicator_color = color;
    }

    void AddCardEnabled(GameObject cardGameobject)
    {
        cardGameobject.GetComponent<CardUI>().SetIndicatorBg(_indicator_color);
        cardGameobject.GetComponent<CardUI>().EnableIndicatorBg();
        _cards_enabled_indicator[_index++] = cardGameobject;
        _indicators_enabled = true;

    }
    public void ResetIndicators()
    {
        foreach(GameObject cardGameobject in _cards_enabled_indicator)
        {
            if(cardGameobject != null && cardGameobject.GetComponent<CardUI>() != null)
                cardGameobject.GetComponent<CardUI>().DisableIndicatorBg();
        }
        _index = 0;
        _total_choses = 0;
        _indicators_enabled = false;
        _cards_enabled_indicator = new GameObject[_cards_enabled_indicator.Length];
        _cards_enabled_indicator_chosen = new bool[_cards_enabled_indicator_chosen.Length];
        _indicator_color = Color.white;
        _select_indicator_color = Color.cyan;
        _asking_from_holder = -1;

    }
    public void ShowInducators(Guid[] cards, Color color)
    {
        print($"showing total cards: {cards.Length}, __cards_enabled size: {_cards_enabled_indicator.Length}, index {_index}");
        int total_card = cards.Length;
        SetIndicatorColor(color);
        foreach(TableCardHolder tch in GameObject.FindObjectsOfType<TableCardHolder>())
        {
            Card tbh_card = tch.GetCard();
            if (tbh_card != null )
            {
                foreach(Guid card_uuid in cards)
                {
                    if(tbh_card._uuid.Equals(card_uuid))
                    {
                        AddCardEnabled(tch.GetCardGameobject());
                        total_card--;
                        break;
                    }
                }
                //return holder.GetCardGameobject();
            }

            if (total_card <= 0)
                break;
        }
    }
    /// <summary>
    /// if card is already selected it will unselect it!
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public int SelectCardFromIndicators(Card card)
    {
        for (int i = 0; i < _cards_enabled_indicator.Length; ++i)
        {
            GameObject cardObj = _cards_enabled_indicator[i];
            if (cardObj == null)
                continue;

            Card c = cardObj.GetComponent<CardUI>().GetCard();
            if (c.Equals(card))
            {
                if(_cards_enabled_indicator_chosen[i])
                {
                    _cards_enabled_indicator_chosen[i] = false;
                    cardObj.GetComponent<CardUI>().SetIndicatorBg(_indicator_color);
                    return i;
                }
                cardObj.GetComponent<CardUI>().SetIndicatorBg(_select_indicator_color);
                _cards_enabled_indicator_chosen[i] = true;
                return i;
            }
        }
        return -1;
    }

    public List<Guid> GetAllSelectedCardUUIDs()
    {
        List<Guid> cards = new List<Guid>();

        for(int i = 0; i < _cards_enabled_indicator_chosen.Length; ++i)
        {
            if (_cards_enabled_indicator_chosen[i])
            {
                cards.Add(GetCardFromSlot(i)._uuid);
                //Console.WriteLine("card added: " + GetCardFromSlot(i)._uuid);
            }
               
        }
        return cards;
    }

    public Card GetCardFromSlot(int i)
    {
        return _cards_enabled_indicator[i] != null ? _cards_enabled_indicator[i].GetComponent<CardUI>().GetCard() : null;
    }

    private Card GetCardFromIndicators(Card card)
    {
        for(int i = 0; i < _cards_enabled_indicator.Length; ++i)
        {
            GameObject cardObj = _cards_enabled_indicator[i];
            if (cardObj == null)
                continue;

            Card c = cardObj.GetComponent<CardUI>().GetCard();
            if(c.Equals(card))
            {
                return c;
            }
        }
        return null;
    }
}
