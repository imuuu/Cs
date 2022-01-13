using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GameServer.Enums;

namespace GameServer
{

    public class Deck
    {
        class DeckCostData
        {
            public int _cost = 0;
            private int _defCost = 0;
            public int _drawMultiplier = 1;
            public int _totalDraws = 0;
            public DCMultIncreaseState _state;

            public DeckCostData(int cost, int drawMultiplier, int totalDraws, DCMultIncreaseState state)
            {
                _cost = cost;
                _defCost = cost;
                _drawMultiplier = drawMultiplier;
                _state = state;
                _totalDraws = totalDraws;
            }
            public void SetCost(int amount)
            {
                _cost = amount;
                _defCost = amount;
            }

            public void ResetValues()
            {
                _cost = _defCost;
                _totalDraws = 0;
            }
        }

        private int _id;
        private String _name;
        private List<Card> _cards;
        private List<Card> _lastCards;
        private bool _locked = false;
        private bool _middleOfchose = false;
        
        private int _drawAmount = 0;

        private Dictionary<DeckBuyType, DeckCostData> _customDeckCostTypeValues = new Dictionary<DeckBuyType, DeckCostData>();
        public Deck(int id, String name, int cost, DeckBuyType costType, int cost_actionPoints,int drawMultiplier, int drawAmount)
        {
            _id = id;
            _name = name;

            _drawAmount = drawAmount;
            _cards = new List<Card>();
            _lastCards = new List<Card>();
            SetCustomCostType(costType, cost, drawMultiplier, DCMultIncreaseState.TOGETHER);
            if(costType != DeckBuyType.ACTION_POINT)
                SetCustomCostType(DeckBuyType.ACTION_POINT, cost_actionPoints, 1, DCMultIncreaseState.ALONE);
        }
        public int GetId()
        {
            return _id;
        }
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public string GetName()
        {
            return _name;
        }

        public Card GetFirstCard()
        {
            Card c = _cards[0];
            _cards.RemoveAt(0);
            return c;
        }
        
        public DeckType GetDeckType()
        {
            return (DeckType)_id;
        }

        public Card RemoveCard(Guid uuid)
        {
            int i = 0;
            for(; i < _cards.Count; ++i)
            {
                if (_cards[i]._uuid == uuid)
                {
                    break;
                }
                   
            }
            Card c = _cards[i];
            _cards.RemoveAt(i);
            
            return c;
        }

        public void RemoveCard(int index)
        {
            _cards.RemoveAt(index);
        }

        /// <summary>
        /// Get "drawAmount" amount of Cards from deck random order. Cards will not be deleted from deck pool! Use RemoveCard(uuid) to remove card
        /// </summary>
        /// <returns></returns>
        public List<Card> GetRandomCards()
        {
            return GetRandomAmountCards(_drawAmount);
        }

        private List<Card> GetRandomAmountCards(int amount)
        {
            List<Card> cards = new List<Card>();
            _lastCards = new List<Card>();
            if (_cards.Count == 0)
                return cards;

            int rolls = amount;
            if (_cards.Count < amount)
                rolls = _cards.Count;

            List<int> indexList = Enumerable.Range(0, rolls).ToList();
            int[] rollNums = new int[rolls];
            Random rand = new Random();
            for(int i = 0; i < rolls; ++i)
            {
                int index = rand.Next(indexList.Count);
                rollNums[i] = indexList[index];
                indexList.RemoveAt(index);
            }

            foreach(int x in rollNums)
            {
                cards.Add(_cards[x]);
                _lastCards.Add(_cards[x]);
            }

            _middleOfchose = true;
            return cards;
        }
        public List<Card> GetCards()
        {
            return _cards;
        }

        public int TotalCards()
        {
            return _cards.Count;
        }

        public bool isEmpty()
        {
            return _cards.Count == 0;
        }

        public void PrintDeck()
        {
            String str = "";
            foreach (Card c in _cards)
            {
                str += c._id + ", ";
            }

            Console.WriteLine($"Deck '{_name}' contains cards: {str}");
        }

        public bool IsLocked()
        {
            return _locked;
        }

        public void SetLock(bool b)
        {
            _locked = b;
        }

        public void ClearLastCards()
        {
            _lastCards.Clear();
        }
        public List<Card> GetLastCards()
        {
            return _lastCards;
        }

        public bool isMiddleOfChose()
        {
            return _middleOfchose;
        }

        public void setMiddleOfChose(bool b)
        {
            _middleOfchose = b;
        }

        public void IncreaseCost(DeckBuyType costType)
        {
            //Console.WriteLine($"increasing cost of {costType}");
            //Dictionary<DeckBuyType, DeckCostData> newValues = new Dictionary<DeckBuyType, DeckCostData>();
            foreach (KeyValuePair<DeckBuyType, DeckCostData> entry in _customDeckCostTypeValues)
            {
                //DeckCostData data = null;
                if (DCMultIncreaseState.TOGETHER == entry.Value._state)
                {
                    int cost = entry.Value._cost;
                    //data = new DeckCostData(entry.Value._cost * entry.Value._drawMultiplier, entry.Value._drawMultiplier, entry.Value._totalDraws, entry.Value._state);
                    entry.Value._cost *= entry.Value._drawMultiplier;
                    //Console.WriteLine($"=====> together costBefore: {cost} after: {entry.Value._cost} with multi: {entry.Value._drawMultiplier}");

                }
                if (DCMultIncreaseState.ALONE == entry.Value._state && entry.Key == costType)
                {
                    //Console.WriteLine($"=====> alone");
                    entry.Value._cost *= entry.Value._drawMultiplier;//new DeckCostData(entry.Value._cost, entry.Value._drawMultiplier, entry.Value._totalDraws, entry.Value._state);
                }

                if (costType == entry.Key)
                    entry.Value._totalDraws += 1;

                //newValues[entry.Key] = data;
            }

            //_customDeckCostTypeValues = newValues;



        }

        public void ResetCost()
        {
            //Dictionary<DeckBuyType, DeckCostData> newValues = new Dictionary<DeckBuyType, DeckCostData>();
            foreach(KeyValuePair<DeckBuyType, DeckCostData> entry in _customDeckCostTypeValues)
            {
                //newValues[entry.Key] = new DeckCostData(entry.Value._defCost,entry.Value._drawMultiplier,0,entry.Value._state);
                entry.Value.ResetValues();
            }

            //_customDeckCostTypeValues = newValues;

        }

        public void SetCustomCostType(DeckBuyType type, int value, int drawMultiplier, DCMultIncreaseState _dCMultIncreaseState)
        {
            //Console.WriteLine($"For deck: {_id} Setting custom cost type for {type} and value is {value}");
            _customDeckCostTypeValues[type] = new DeckCostData(value, drawMultiplier, 0 ,_dCMultIncreaseState);
        }

        public int GetCost(DeckBuyType type)
        {
            //Console.WriteLine($"Asking deck cost from type: {type}");
            if (_customDeckCostTypeValues.ContainsKey(type))
                return _customDeckCostTypeValues[type]._cost;

            return -1;
        }

        public void SetCost(DeckBuyType type, int cost)
        {
            if (_customDeckCostTypeValues.ContainsKey(type))
                _customDeckCostTypeValues[type].SetCost(cost);
        }

        public bool isPlayerAbleToBuy(Player player, DeckBuyType type)
        {
            int cost = GetCost(type);
            
            if (cost < 0)
            {
                Console.WriteLine("class:Deck : The deckBuyType doesnt exist");
                return false;
            }
               

            switch (type)
            {
                case DeckBuyType.GOLD:
                    {
                        if (player.GetMoney() >= cost)
                            return true;
                        break;
                    }
                case DeckBuyType.MANA:
                    {
                        if (player.GetMana() >= cost)
                            return true;
                        break;
                    }
                case DeckBuyType.HEALTH:
                    {
                        if (player.GetHero()._hp >= cost)
                            return true;
                        break;
                    }
                case DeckBuyType.ACTION_POINT:
                    {
                        if (player.GetRoundActionPoints() >= cost)
                            return true;
                        break;
                    }
            }

            return false;

        }

        public void AddPlayerCost(Player player, DeckBuyType type)
        {
            int cost = GetCost(type) * -1;
            switch (type)
            {
                case DeckBuyType.GOLD:
                    {
                        player.AddMoney(cost);
                        break;
                    }
                case DeckBuyType.MANA:
                    {
                        player.AddMana(cost);
                        break;
                    }
                case DeckBuyType.HEALTH:
                    {
                        player.GetHero().AddDamage(cost);
                        break;
                    }
                case DeckBuyType.ACTION_POINT:
                    {
                        player.AddRoundActionPoints(cost);
                        break;
                    }

            }

        }
    }
}
