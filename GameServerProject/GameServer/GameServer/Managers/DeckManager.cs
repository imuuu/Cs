using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;

namespace GameServer.Managers
{
    class DeckManager
    {
        public void DeckPurchase(Player player, Deck deck, DeckBuyType buyType)
        {
            if (deck == null)
            {
                Console.WriteLine("Deck doesnt exist");
                return;
            }

            Table table = player.GetTable();
            DeckType dType = (DeckType)deck.GetId();
            Console.WriteLine("deck id: " + dType.ToString());

            Deck middleofChoseDeck = table.isAnyDeckMiddleOfChose();
            if (middleofChoseDeck != null) // if some how client havent chose card from deck and its middle of chose
            {
                ServerSend.SendCardChoses(player._id, middleofChoseDeck.GetId(), middleofChoseDeck.GetLastCards());
                Console.WriteLine($"Deck named {middleofChoseDeck.GetName()} is middle of chose!");
                return;
            }

            if (deck.isEmpty())
            {
                Console.WriteLine("Deck is empty");
                Server._clients[player._id].SendCardToHand(Server._cardColManager.GetCard(0)); // Empty deck
                return;
            }

            if (deck.IsLocked())
            {
                Console.WriteLine("Deck is locked");
                return;
            }

            if (!deck.isPlayerAbleToBuy(player, buyType))
            {
                Console.WriteLine($"Player (ID: {player._username}) doesn't have {buyType}! Deck Cost: {deck.GetCost(buyType)} {buyType}");
                return;
            }

            if (dType == DeckType.INCOME)
            {
                Console.WriteLine($"Income increased");
                player.AddMoneyIncome(Constants.DECK_INCOME_AMOUNT);
                deck.RemoveCard(0);
                DeckCostHandler(player, buyType, deck);
                RemoveCardFromOthersDecks(player, deck, buyType);
                return;
            }

            if (!player.ReduceRoundActionPoints(deck.GetCost(DeckBuyType.ACTION_POINT)))
            {
                return;
            }
            

            if (dType == DeckType.MANA)
            {
                Console.WriteLine($"Mana cap increased");
                player.AddManaCap(1);
                deck.RemoveCard(0);
                DeckCostHandler(player, buyType, deck);
                RemoveCardFromOthersDecks(player, deck, buyType);
                return;
            }

            if (dType == DeckType.OWN) // own deck
            {
                Card c = deck.RemoveCard(deck.GetRandomCards()[0]._uuid);
                if (c != null)
                {
                    c._state = CardState.HAND;
                    table.PrintDeck(deck.GetId());
                    player.GetHand().AddCard(c);
                    player.GetHand().PrintCards();
                    Server._clients[player._id].SendCardToHand(c);                  
                    deck.setMiddleOfChose(false);
                    DeckCostHandler(player, buyType, deck);
                    return;
                }
                return;
            }

            

            if(dType == DeckType.VICTORY)
            {
                Console.WriteLine("bought from victory deck");
                //player.AddVictoryPoints(1);
                deck.AddPlayerCost(player, buyType);
                deck.IncreaseCost(buyType);
                deck.SetCost(buyType, deck.GetCost(buyType));
                deck.ResetCost();
                //deck.RemoveCard(0);
                ServerSend.SendPlayerData(player._id);
                ServerSend.SendDeckData(player._id, deck);

                //RemoveCardFromOthersDecks(player, deck, buyType);
                //return;
            }else
            {
                DeckCostHandler(player, buyType, deck);
            }

            //sending choses for cards
            List<Card> cards = deck.GetRandomCards();

            //Console.WriteLine("founded cards: " + cards.Count);
            //Console.WriteLine($"card1{ cards[0]}, card 2 { cards[1]} card 3 { cards[2]}");

            RemoveCardFromOthersDecks(player, deck, buyType); //if reroll posibility happens in future this cant be here!

            if (cards.Count == 1)
            {
                PutChosenCardToOwnDeck(player, cards[0]._uuid, deck);            
                return;
            }
            ServerSend.SendCardChoses(player._id, deck.GetId(), cards);
            return;
        }

        void DeckCostHandler(Player player, DeckBuyType buyType, Deck deck)
        {
            deck.AddPlayerCost(player, buyType);
            deck.IncreaseCost(buyType);
            ServerSend.SendPlayerData(player._id);
            ServerSend.SendDeckData(player._id, deck);

            if(buyType == DeckBuyType.HEALTH)
            {
                Server._heroManager.SendHeroesToClients(player._id);
            }
        }

        public void PutChosenCardToOwnDeck(Player player, Guid card_uuid, Deck deck)
        {
            Card card = deck.RemoveCard(card_uuid);
            card._state = CardState.DECK;
            player.GetTable().GetDeck(0).AddCard(card);
            player.GetTable().GetDeck(0).PrintDeck();
            deck.setMiddleOfChose(false);
            ServerSend.SendDeckData(player._id, deck);
            ServerSend.SendDeckData(player._id, player.GetTable().GetDeck(0));
        }

        public void RemoveCardFromOthersDecks(Player pDrawer, Deck deck, DeckBuyType buyType)
        {
            Player opponent;
            Deck oDeck;
            Random rand = new Random();
            foreach (Client client in Server._clients.Values)
            {
                if (client._id == pDrawer._id)
                    continue;

                if (client._tcp._socket == null)
                    continue;

                opponent = client._player;

                oDeck = opponent.GetTable().GetDeck(deck.GetDeckType());
                oDeck.RemoveCard(rand.Next(oDeck.TotalCards()));
                if (oDeck.GetDeckType() == DeckType.VICTORY)
                {
                    oDeck.IncreaseCost(buyType);
                    oDeck.SetCost(buyType, deck.GetCost(buyType));
                    oDeck.ResetCost();
                }
                    ServerSend.SendDeckData(opponent._id, oDeck);
            }
        }
    }
}
