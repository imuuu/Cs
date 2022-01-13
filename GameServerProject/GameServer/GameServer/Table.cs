using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Managers;
using GameServer.Enums;
namespace GameServer
{
   
    public class Table
    {
        Player _player;
        private Dictionary<int, Deck> _decks;
        private CardCollectionManager _cardColManager;
       
        public Table(Player player, CardCollectionManager cardColManager)
        {
            _decks = new Dictionary<int, Deck>();
            _cardColManager = cardColManager;
            _player = player;
            
            //InitDecks();
        }
        void DeckSorter(Card card)
        {
            if (card._manaCost > 0 && card._manaCost < 4)
            {
                _decks[1].AddCard(card);
                return;
            }

            if (card._manaCost > 3 && card._manaCost < 7)
            {
                _decks[2].AddCard(card);
                return;
            }

            if (card._manaCost > 6)
            {
                _decks[3].AddCard(card);
                return;
            }
        }
        public void InitDecks(int[] cardIds)
        {

            //deck id, name, cost, deckbuytype, cost_actionPoints, drawMultiplier, drawamount
            _decks[0] = new Deck(0,"Own"        , 50,   DeckBuyType.GOLD, 0,     2, 1); 
            _decks[1] = new Deck(1,"Low Cost"   , 50,   DeckBuyType.GOLD, 1,     1, 3);
            _decks[2] = new Deck(2,"Mid Cost"   , 100,    DeckBuyType.GOLD, 1,     1, 3); 
            _decks[3] = new Deck(3,"High Cost"  , 200,    DeckBuyType.GOLD, 1,     1, 3); 
            _decks[4] = new Deck(4,"Victory"    , 20,   DeckBuyType.GOLD, 1,     2, 3); 
            _decks[5] = new Deck(5,"Spell"      , 350,  DeckBuyType.GOLD, 3,     1, 3); 
            _decks[6] = new Deck(6,"Mana"       , 50,   DeckBuyType.GOLD, 1,     1, 1); 
            _decks[7] = new Deck(7,"Graveyard"  , 999,  DeckBuyType.GOLD, 1,     1, 1);
            _decks[8] = new Deck(8,"Income"     ,   1,  DeckBuyType.ACTION_POINT,0,1,1);

            foreach (int cardID in cardIds) { DeckSorter(_cardColManager.GetCard(cardID));}

            //int state = 0;
            //for(int i = 0; i < cardIds.Length; ++i)
            //{
            //    if (i != 0 && i % 20 == 0)
            //    {
            //        //PrintDeck(state);
            //        state++;
            //    }

            //    if (cardIds[i] == 0)
            //        continue;
            //    Card c = _cardColManager.GetCard(cardIds[i]);
            //    _decks[state].AddCard(c);
            //}
            for (int i = 0; i < 20; i++)
            {
                _decks[4].AddCard(_cardColManager.GetCard(57));
                _decks[5].AddCard(_cardColManager.GetCard(58));
                _decks[6].AddCard(_cardColManager.GetCard(0));
                _decks[8].AddCard(_cardColManager.GetCard(0));
                
            }
            for(int cardID = 1; cardID < 11; ++cardID)
            {
                _decks[0].AddCard(_cardColManager.GetCard(cardID));
                //_decks[0].AddCard(_cardColManager.GetCard(cardID));
            }
            Console.WriteLine("Player decks has been set");
           
        }

        public void SendDecks()
        {
            foreach (Deck deck in _decks.Values)
            {
                ServerSend.SendDeckData(_player._id, deck);
            }
        }

        public void PrintDeck(int deckID)
        {
            _decks[deckID].PrintDeck();
        }

        public Deck GetDeck(DeckType deckID)
        { 
            if(_decks.ContainsKey((int)deckID))
            {
                return _decks[(int)deckID];
            }
            return null;
            
        }

        public Deck isAnyDeckMiddleOfChose()
        {
            foreach(Deck deck in _decks.Values)
            {
                if(deck.isMiddleOfChose())
                {
                    return deck;
                }
            }

            return null;
        }

        public void ResetDecksCost()
        {
            foreach (Deck deck in _decks.Values)
            {
                deck.ResetCost();
                ServerSend.SendDeckData(_player._id, deck);
            }
        }

       
    }
}
