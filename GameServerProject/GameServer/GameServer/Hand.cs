using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class Hand
    {
        Player _player;
        List<Card> _cards;
        int _handSize = 20; //TODO pitää tehdä este tälle serversidelle ettei ylity
        public Hand(Player player)
        {
            _player = player;
            _cards = new List<Card>();
        }
        public void AddCard(Card card)
        {
            card.SetOwner(_player._id);
            _cards.Add(card);
        }

        public Card GetCard(Card card)
        {
            return _cards.Find(x => x == card);
        }

        public Card GetCard(int id)
        {
            return _cards.Find(x => x._id == id);
        }

        public Card GetCard(Guid uuid)
        {
            return _cards.Find(x => x._uuid == uuid);
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
            ServerSend.RemoveCardFromHand(_player._id, card);
        }

        public void PrintCards()
        {
            string str = "In hand: ";
            foreach(Card c in _cards)
            {
                str += c._id+", ";
            }
            Console.WriteLine(str);

        }

        public int TotalCards()
        {
            return _cards.Count;
        }

        public int GetHandSize()
        {
            return _handSize;
        }
    }
}
