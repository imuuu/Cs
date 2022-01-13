using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Managers;
using System.Reflection;
using GameServer.Enums;
using System.Linq;

namespace GameServer.Managers
{
    public class BuffCardManager
    {
        int _id;
        int[] _clients;
        private CardCollectionManager _cardColManager;
        //private Action<Card, Buff>[] _effectAreas = new Action<Card, Buff>[8];

        private Dictionary<Guid, List<Card>> _all_buffed_cards = new Dictionary<Guid, List<Card>>(); //buffs uuid and cards from buff
        
        private Dictionary<int, ChosenPlate> _client_chosen_buff = new Dictionary<int, ChosenPlate>();
        private Dictionary<int, List<BuffPlate>> _client_buffs_in_hold = new Dictionary<int, List<BuffPlate>>();
        private class ChosenPlate
        {
            public Buff _buff;
            public Card _buff_giver;
            public List<Card> _cards;
        }
        private class BuffPlate
        {
            public Buff _buff;
            public Card _card;
            public BuffPlate(Card buffedCard, Buff buff)
            {
                _card = buffedCard;
                _buff = buff;
            }
        }
        public BuffCardManager(int id, int[] clients ,CardCollectionManager cardcolManager)
        {
            _id = id;
            _clients = clients;
            _cardColManager = cardcolManager;
            Events.onCardDestroy.AddListener(OnCardDestroyid);
            Events.onRoundStart.AddListener(OnRoundStart);
            Events.onCardTakeDamage.AddListener(OnCardTakeDamage);
            Events.onCardPlay.AddListener(OnCardPlay);
            Events.onCardHeal.AddListener(OnCardHeal);
           
        }  
        void OnCardPlay(Card card)
        {
            BuffSenderTriggerSorter(card, BuffTriggerType.ON_PLAY);
        }

        public bool IsClientMiddleOfChosenFromIndicatorCards(int clientId)
        {
            return _client_chosen_buff.ContainsKey(clientId);
        }

        void OnRoundStart(int client)
        {
            TriggerBuffOnHold(client);
            TriggerBuffRoundChecker(client);
        }

        /// <summary>
        /// Putting buff to hold to triggered when players round comes
        /// </summary>
        /// <param name="card"></param>
        /// <param name="buff"></param>
        void PutBuffOnHold(Card card, Buff buff)
        {
            if (buff is IBuffRound)
                ((IBuffRound)buff)._total_sustain_rounds++;

            BuffPlate buffplate = new BuffPlate(card, buff);
           if(_client_buffs_in_hold.ContainsKey(card.GetOwner()))
           {
                _client_buffs_in_hold[card.GetOwner()].Add(buffplate);
                return;
           }
            List<BuffPlate> cards = new List<BuffPlate>() { buffplate };
            _client_buffs_in_hold[card.GetOwner()] = cards;
        }

        /// <summary>
        /// Try to check if there has been some buffs on hold that will be triggered when player has his own turn
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public bool TriggerBuffOnHold(int clientID)
        {
            //Console.WriteLine("Searching buffs on hold.. from client: "+client);
            if(_client_buffs_in_hold.ContainsKey(clientID))
            {
                //Console.WriteLine("==> FOUND!");
                BuffPlate plate = _client_buffs_in_hold[clientID][0];
                BuffTypeSender(plate._card, plate._buff);

                _client_buffs_in_hold[clientID].RemoveAt(0);
                
                if (_client_buffs_in_hold[clientID].Count == 0)
                    _client_buffs_in_hold.Remove(clientID);
                return true;
            }
            
            _client_chosen_buff.Clear();
            
            return false;
        }

        public void TriggerBuffRoundChecker(int clientID)
        {

            foreach(Card bfCard in Server._clients[clientID]._player.GetBattlefield().GetAllLinesCards())
            {
                if (bfCard == null)
                    continue;

                if (!(bfCard is MonsterCard))
                    continue;

                MonsterCard card = (MonsterCard)bfCard;

               foreach(Buff buff in card.GetAllBuffs())
               {
                   if((buff as IBuffRound) != null)
                    {
                        if(--((IBuffRound)buff)._total_sustain_rounds <= 0)
                        {
                            Console.WriteLine("=========> BuffRound has been expired!");
                            if (!_all_buffed_cards.ContainsKey(buff._uuid))
                                break;

                            foreach(Card c in _all_buffed_cards[buff._uuid])
                            {
                                RemoveBuff((MonsterCard)c, buff);
                                Console.WriteLine($"================> Buff removed from card: {c._name}!");
                            }
                            UpdateBattleFieldCards(_all_buffed_cards[buff._uuid].ToArray());
                            _all_buffed_cards.Remove(buff._uuid);
                        }
                    }
                   if(_all_buffed_cards.ContainsKey(buff._uuid))
                    {
                        if(_all_buffed_cards[buff._uuid].Count == 0)
                        {
                            Console.WriteLine($"There isnt any buffs from this on field");
                            _all_buffed_cards.Remove(buff._uuid);
                        }
                    }
               }
            }
        }
        Card RemoveBuff(MonsterCard card, IBuff buff)
        {
            card.RemoveBuff(buff);
            Buff deBuff = buff.Clone();
            deBuff._value = deBuff._value * -1;
            return BuffTheCard(card, deBuff);
        }
        /// <summary>
        /// Adds Card who gives buff and its buffed cards.. Gives access all buffed cards from specific card.
        /// </summary>
        /// <param name="card_who_buffs"></param>
        /// <param name="card_who_gets_buff"></param>
        Card AddBuffedCard(MonsterCard card_who_gets_buff, Buff buff)
        {
            //card_who_gets_buff.AddBuffGiver(card_who_buffs, buff);
           
            BuffPlate bPlate = new BuffPlate(card_who_gets_buff, buff);
            if (_all_buffed_cards.ContainsKey(buff._uuid))
            {
                //Console.WriteLine($"Contains card: {card_who_buffs._name} and uuid: {card_who_buffs._uuid}");
                _all_buffed_cards[buff._uuid].Add(card_who_gets_buff.AddBuff(buff));
                return card_who_gets_buff;
            }
            
            //add buff its self can be added here and remeber this can be called many times from same metod!
            List<Card> cards = new List<Card>();
            cards.Add(card_who_gets_buff.AddBuff(buff));
            _all_buffed_cards[buff._uuid] = cards;
            return card_who_gets_buff;
        }
        void OnCardDestroyid(Card buff_giver_card)
        {
            //Console.WriteLine("Card has been destroyed.. searching buffs");
            if(buff_giver_card is IBuffCard)
            {
                foreach (Buff buff in ((IBuffCard)buff_giver_card).GetAllSettedBuffs())
                {
                    if (buff._state == BuffState.TEMP)
                    {
                        if (_all_buffed_cards.ContainsKey(buff._uuid))
                        {
                            foreach (Card card in _all_buffed_cards[buff._uuid])
                            {
                                RemoveBuff((MonsterCard)card, buff);
                            }

                            UpdateBattleFieldCards(_all_buffed_cards[buff._uuid].ToArray());
                            _all_buffed_cards.Remove(buff._uuid);
                        }
                    }


                }
            }
            

            BuffSenderTriggerSorter(buff_giver_card, BuffTriggerType.ON_DEAD);
        }

        void OnCardTakeDamage(Card card)
        {
            BuffSenderTriggerSorter(card, BuffTriggerType.ON_TAKE_DAMAGE);
        }

        void OnCardHeal(Card card)
        {
            BuffSenderTriggerSorter(card, BuffTriggerType.ON_HEAL);
        }

        void BuffSenderTriggerSorter(Card buff_giver_card, BuffTriggerType trigger)
        {
            if(buff_giver_card is IBuffCard)
            {
                foreach (Buff buff in ((IBuffCard)buff_giver_card).GetAllSettedBuffs())
                {
                    if (buff._trigger == trigger)
                    {                       
                        if (Server._gameScene.GetRoundManager(_id).GetPlayerWhosTurn() != buff_giver_card.GetOwner() && ((buff as IBuffChosenCard) != null))
                        {
                            Console.WriteLine("Buff has been put to hold to wait choses of card holder!");
                            PutBuffOnHold(buff_giver_card, buff);
                            return;
                        }
                        BuffTypeSender(buff_giver_card, buff);
                    }
                }
            }
            
        }

        private Card FilterTheCard(MonsterCard card, Buff buff)
        {
            if (card == null)
                return null;

            if (buff._buffingFaction != CardFaction.NONE && card._faction != buff._buffingFaction)//
            {
                Console.WriteLine($"{card._name}: Cant buff bc not same faction");
                return null;
            }

            if (buff._buffingCardType != CardType.NONE && card._type != buff._buffingCardType)
            {
                Console.WriteLine($"{card._name}: Cant buff bc not same type");
                return null;
            }

            //if ((buff as BuffChosenCard) != null && ((BuffChosenCard)buff)._buffingCardType != CardType.NONE && card._type != ((BuffChosenCard)buff)._buffingCardType)
            //{
            //    Console.WriteLine($"{card._name}: Chosen card bc not same type");
            //    return null;
            //}

            //if ((buff as BuffChosenCard) != null && ((BuffChosenCard)buff)._buffingFaction != CardFaction.NONE && card._faction != ((BuffChosenCard)buff)._buffingFaction)
            //{
            //    Console.WriteLine($"{card._name}: Chosen card bc not same faction");
            //    return null;
            //}


            return card;
        }

        private MonsterCard BuffTheCard(MonsterCard card, Buff buff)
        {
            Console.WriteLine($"{card._name} getting buff..");
            switch (buff._type)
            {
                case BuffType.NONE:
                    Console.WriteLine($"===========> NONE:  {buff._value}");
                    break;
                case BuffType.GIVE_OR_TAKE_HEALTH:
                    Console.WriteLine($"===========> health: {buff._value}");
                    card.TakeOrAddHealth(buff._value);                   
                    break;
                case BuffType.GIVE_OR_TAKE_ATTACK:
                    Console.WriteLine($"===========> attack: {buff._value}");
                    card.TakeOrAddAttack(buff._value);
                    break;
                
            }
            return card;
        }
        
        private void SendIndicatorCards(Card card_buffer, Buff buff, List<Card> cards)
        {

            if (cards.Count <= 0)
                return;

            ChosenPlate cc = new ChosenPlate();
            cc._buff_giver = card_buffer;
            cc._buff = buff;
            cc._cards = cards;
            _client_chosen_buff.Add(card_buffer.GetOwner(), cc);
            int total_choses = ((IBuffChosenCard)buff)._total_choses > cards.Count ? cards.Count : ((IBuffChosenCard)buff)._total_choses;
            
            ServerSend.SendCardIndicators(card_buffer.GetOwner(), cards.Select(u => u._uuid).ToArray(), total_choses, -1);
        }

        public void ChosenCard(int clientId, List<Guid> chosen_card_uuids)
        {
            
            if (!_client_chosen_buff.ContainsKey(clientId))
                return;
            ChosenPlate plate = _client_chosen_buff[clientId];
            List<Card> buffed_cards = new List<Card>();
            foreach (Guid uuid in chosen_card_uuids)
            {
                foreach (Card card in plate._cards)
                {
                    if (buffed_cards.Contains(card))
                    {
                        continue;
                    }
                    if (buffed_cards.Count >= ((IBuffChosenCard)plate._buff)._total_choses)
                    {
                        Console.WriteLine("Total choses has gone over the value!");
                        break;
                    }

                    if (card._uuid.Equals(uuid))
                    {
                        Card c = BuffTheCard((MonsterCard)card, plate._buff);
                        buffed_cards.Add(AddBuffedCard((MonsterCard)c, plate._buff));
                       
                    }
                }
            }
            _client_chosen_buff.Remove(clientId);
            UpdateBattleFieldCards(buffed_cards.ToArray());
        }

        void UpdateBattleFieldCards(Card[] cards)
        {         
            foreach(int clientID in _clients)
            {
                ServerSend.SendBattlefieldCardUpdate(clientID, cards);
            }
        }

        public List<Card> GetCardsOfBuffPattern(Card card, Buff buff)
        {
            //Console.WriteLine("card: "+card.GetOwner());
            card.PrintData();

            Battlefield bf = Server._clients[card.GetOwner()]._player.GetBattlefield(); // error happend
            int[] card_slot_ids = bf.GeneratePattern(card._battlefieldSlotID, buff._include_self, buff._pattern);
            List<Card> filtered_cards = new List<Card>();
            foreach (int card_slot in card_slot_ids)
            {
                Card filtered_card = FilterTheCard((MonsterCard)bf.GetCard(card_slot), buff);
                if (filtered_card != null)
                    filtered_cards.Add(filtered_card);
            }
            return filtered_cards;
        }

        /// <summary>
        /// Card is buffer card
        /// </summary>
        /// <param name="card"></param>
        /// <param name="buff"></param>
        public  void BuffTypeSender(Card card, Buff buff)
        {
            Battlefield bf =  Server._clients[card.GetOwner()]._player.GetBattlefield();
            int[] card_slot_ids = bf.GeneratePattern(card._battlefieldSlotID, buff._include_self, buff._pattern);
            List<Card> buffed_cards = new List<Card>();
            List<Card> filtered_cards = new List<Card>();
            foreach (int card_slot in card_slot_ids)
            {
                Card filtered_card = FilterTheCard((MonsterCard)bf.GetCard(card_slot), buff);

                if (filtered_card == null)
                    continue;
                
                filtered_cards.Add(filtered_card);
                if ((buff as IBuffChosenCard) != null)
                {
                    continue;
                }

                Card getting_buff_card = BuffTheCard((MonsterCard)filtered_card, buff); ;
                if (getting_buff_card == null)
                    continue;

                buffed_cards.Add(AddBuffedCard((MonsterCard)getting_buff_card, buff));

            }

            if ((buff as IBuffChosenCard) != null)
            {
                SendIndicatorCards(card, buff, filtered_cards);
                return;
            }
            UpdateBattleFieldCards(buffed_cards.ToArray());
        }
        

        
    }
}