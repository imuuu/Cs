using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;

namespace GameServer
{
    public class Battlefield
    {
        private Player _owner;
        private Card[] _lines;
        int _total_card_front = 0;
        int _total_card_back = 0;
        public Battlefield(Player owner, int cardsInFront, int cardsInBack)
        {
            _owner = owner;
            _total_card_front = cardsInFront;
            _total_card_back = cardsInBack;
            _lines = new Card[_total_card_front + _total_card_back];
            Events.onRoundStart.AddListener(OnRoundStart);
        }

        public bool AddCard(int index, Card card)
        {

            if (IsFrontline(index))
            {
                _lines[index] = card;
                _lines[index]._state = CardState.FRONTLINE;
                _lines[index]._battlefieldSlotID = index;
                Console.Write($"{card._name} added to slot: {index} in frontline");
                return true;
            }
            if (IsBackline(index))
            {
                _lines[index] = card;
                _lines[index]._state = CardState.BACKLINE;
                _lines[index]._battlefieldSlotID = index;
                Console.Write($"{card._name} added to slot: {index} in backline");
                return true;
            }
            return false;

        }

        void OnRoundStart(int clientID)
        {
            if (clientID != _owner._id)
                return;

            for(int i = 0; i < _lines.Length; ++i)
            {
                Card c = _lines[i];
                if (c == null)
                    continue;

                if(c is IBuffCard)
                {
                    ((IBuffCard)c).ResetAmountOfAttacks();
                }
            }
        }

        public bool IsFrontline(int index)
        {
            return (index >= 0 && index < _total_card_front);
            
        }

        public bool IsBackline(int index)
        {
            return (index > _total_card_front-1 && index < _lines.Length);
        }

        public Card GetCard(int slot)
        {
            if (slot < 0 || slot >= _lines.Length)
                return null;
            //Console.WriteLine($"Getting card from slot: {slot} => {(_lines[slot] != null ? _lines[slot]._name : "null")} ");
            return _lines[slot];
        }

        public Card[] GetAllLinesCards()
        {
            return _lines;
        }

        public bool IsValidSlot(int slot)
        {
            return slot < _lines.Length;
        }

        public Card GetCard(Guid card_uuid)
        {
            for(int i = 0; i < _lines.Length; ++i)
            {
                //Console.WriteLine($"Checking card {i} : {_lines[i]} ");
                if (_lines[i] != null &&  _lines[i]._uuid.Equals(card_uuid))
                {
                    return _lines[i];
                }
            }      
            return null;
        }

        public Card RemoveCard(Card card)
        {           
            for (int i = 0; i < _lines.Length; ++i)
            {
                if (_lines[i] != null && _lines[i]._uuid.Equals(card._uuid))
                {
                    Card c = _lines[i];
                    _lines[i] = null;
                    return c;
                }
            }
            return null;
        }
        /// <summary>
        /// Gives 2 cards in same vertical position return[frontline card, backline card]
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public int[] GetBattlelineVerticalSlots(int slot)
        {
            //4 - backline amount = 0
            //7 - 4
            int[] arr = new int[2];
            if (slot > _total_card_front - 1)
            {
                arr[0] = slot - _total_card_back;
                arr[1] = slot;              
            }else
            {
                arr[0] = slot;
                arr[1] = slot + _total_card_front;
            }
            

            //Console.WriteLine($"Asking: {slot} Frontline slot: {arr[0]} and Backline slot: {arr[1]}");
            return arr;
        }
        /// <summary>
        /// Gives 2 cards in same horizontal position return[left,right]
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public int[] GetCardsSideCardsSlots(int slot)
        {
            int[] arr = new int[2];

            arr[0] = slot - 1;
            arr[1] = slot + 1;
                
            if (slot == 0 || slot == _total_card_front)
                arr[0] = -1;
            if (slot == _lines.Length - 1)
                arr[1] = -1;

            
            return arr;
        }

        public bool CanCardBeAttacked(Card card)
        {
            if (card._state == CardState.FRONTLINE)
            {
                Console.WriteLine("Card is frontline, can attack");
                return true;
            }
               

            if (GetCard(GetBattlelineVerticalSlots(card._battlefieldSlotID)[0]) == null)
            {
                Console.WriteLine("Front clear! can attack");
                return true;
            }
            Console.WriteLine("Not able to attack");

            return false;
        }

        public bool CanAttackHero()
        {
            for(int i = 0; i < _lines.Length; ++i)
            {
                if (_lines[i] != null)
                    return false;
            }
            return true;
        }
        public int GetHolderID(int slot)
        {
            if (slot <= _total_card_front)
                return slot;

            return slot - _total_card_front;
        }
        public int GetHolderID(Card card)
        {
            return GetHolderID(card._battlefieldSlotID);
        }
        public bool isHolder(int holderID)
        {
            return (holderID >= 0 && holderID < _lines.Length);
        }
        public CardHolderType GetHolderType(int slot)
        {
            //Console.WriteLine("asking slot: " + slot);
            if (slot < _total_card_front)
                return CardHolderType.FRONTLINE;

            return CardHolderType.BACKLINE;
        }
        public CardHolderType GetHolderType(Card card)
        {
            return GetHolderType(card._battlefieldSlotID);
        }

        public void PrintBattleField()
        {
            for(int i = 0; i < _lines.Length; ++i)
            {
                if(_lines[i] != null)
                {
                    Console.WriteLine($"{i} Line: {_lines[i]._uuid}");
                }
                else
                {
                    Console.WriteLine($"{i} Line: {_lines[i]}");
                }
            }
               
        }
        

        public int[] GeneratePattern(int startSlot, bool include_startSlot, Pattern pattern)
        {
            List<int> card_slots = new List<int>();           
            switch (pattern)
            {
                case Pattern.FRONT_CARD:
                    foreach (int slot in GetBattlelineVerticalSlots(startSlot))
                    {
                        if (!include_startSlot)
                            if (slot == startSlot)
                                continue;

                        card_slots.Add(slot);
                    }

                    break;
                case Pattern.BEHIND_CARD:
                    return GeneratePattern(startSlot, include_startSlot, Pattern.FRONT_CARD);
                case Pattern.SIDE_CARDS:
                    foreach (int slot in GetCardsSideCardsSlots(startSlot))
                    {
                        if (slot < 0)
                            continue;

                        if (!include_startSlot)
                            if (slot == startSlot)
                                continue;

                        card_slots.Add(slot);
                    }
                    break;
                case Pattern.AROUND_CARDS:
                    foreach (int slot in GetBattlelineVerticalSlots(startSlot))
                    {
                        foreach (int sideSlot in GetCardsSideCardsSlots(slot))
                        {
                            card_slots.Add(sideSlot);
                        }

                        if (!include_startSlot)
                            if (slot == startSlot)
                                continue;

                        card_slots.Add(slot);
                    }
                    break;
                case Pattern.CROSS_CARDS:
                    int[][] slots = new int[3][];
                    slots[0] = GeneratePattern(startSlot, include_startSlot, Pattern.FRONT_CARD);
                    slots[1] = GeneratePattern(startSlot, include_startSlot, Pattern.BEHIND_CARD);
                    slots[2] = GeneratePattern(startSlot, include_startSlot, Pattern.SIDE_CARDS);

                    foreach (int[] i in slots)
                    {
                        foreach (int l in i)
                        {
                            if (card_slots.Contains(l))
                                continue;

                            card_slots.Add(l);
                        }
                    }
                    break;
                case Pattern.ALL_CARDS:
                    foreach (Card card in GetAllLinesCards())
                    {
                        card_slots.Add(card._battlefieldSlotID);
                    }
                    break;
                case Pattern.FRONTLINE_CARDS:
                    foreach (Card card in GetAllLinesCards())
                    {
                        if (card._state == CardState.FRONTLINE)
                            card_slots.Add(card._battlefieldSlotID);
                    }
                    break;
                case Pattern.BACKLINE_CARDS:
                    foreach (Card card in GetAllLinesCards())
                    {
                        if (card._state == CardState.BACKLINE)
                            card_slots.Add(card._battlefieldSlotID);
                    }
                    break;
                case Pattern.SAME_SLOT:
                    {
                        card_slots.Add(startSlot);
                        break;
                    }
            }

            if (card_slots.Count > 0)
                return card_slots.ToArray();

            return null;
        }

    }
}
