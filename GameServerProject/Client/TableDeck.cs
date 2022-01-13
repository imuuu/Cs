using System.Collections;
using UnityEngine;
using TMPro;
public class TableDeck : Deck
{
    TextMeshProUGUI _textCost_Gold;
    TextMeshProUGUI _textCost_Mana;
    TextMeshProUGUI _textCost_Health;
    TextMeshProUGUI _textCost_ActionPoints;
    TextMeshProUGUI _textTotalCards;
    private void Awake()
    {
        _textCost_Gold = this.gameObject.transform.Find("T_Cost_Gold").GetComponent<TextMeshProUGUI>();
        _textCost_Mana = this.gameObject.transform.Find("T_Cost_Mana").GetComponent<TextMeshProUGUI>();
        _textCost_Health = this.gameObject.transform.Find("T_Cost_Health").GetComponent<TextMeshProUGUI>();
        _textCost_ActionPoints = this.gameObject.transform.Find("T_Cost_ActionPoint").GetComponent<TextMeshProUGUI>();
        _textTotalCards = this.gameObject.transform.Find("T_Cards").GetComponent<TextMeshProUGUI>();
    }
    public override void Clicked()
    {
        //if(_cost > GameManager._players[Client._instance._myId].GetMoney())
        //{
        //    Events.notEnoughMoney.Invoke();
        //    return;
        //}

        if(IsOneBuyType())
        {
            ClientSend.DeckClicked(_id, GetFirstBuyType());
        }
        else
        {
            Debug.Log("Deck has Multiple buytypes.. not implemented!");
        }
    }
            

    //public void SetCost(int cost, DeckBuyType buyType)
    //{
    //    SetCostOfBuyType(cost, buyType);
        

    //}
    public void SetCostOfBuyType(int cost, DeckBuyType buyType)
    {
        TextMeshProUGUI text= null;
        Color color = Color.red;
        PlayerManager pManager = GameManager._players[Client._instance._myId];
        switch (buyType)
        {
            case DeckBuyType.GOLD:
                {
                    _cost_gold = cost;
                    _textCost_Gold.text = cost.ToString();
                    text = _textCost_Gold;
                    if (pManager.GetMoney() >= cost)
                    {
                        color = Color.green;
                    }
                    break;
                }
            case DeckBuyType.HEALTH:
                {
                    _cost_health = cost;
                    text = _textCost_Health;
                    if (pManager.GetHeroHealth() >= cost)
                    {
                        color = Color.green;
                    }
                    break;
                }
            case DeckBuyType.MANA:
                {
                    _cost_mana = cost;
                    text = _textCost_Mana;
                    if (pManager.GetMana() >= cost)
                    {
                        color = Color.green;
                    }
                    break;
                }
            case DeckBuyType.ACTION_POINT:
                {
                    _cost_actionPoint = cost;
                    text = _textCost_ActionPoints;
                    if (pManager.GetRoundActionPoints() >= cost)
                    {
                        color = Color.green;
                    }
                    break;
                }
        }

        text.text = cost.ToString();
        text.color = color;
    }
    public void SetTotalCards(int amount)
    {
        _textTotalCards.text = amount.ToString();
    }

}