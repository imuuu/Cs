using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Managers;
namespace GameServer.Managers
{
    public partial class BattlefieldManager
    {
        int _id;
        int[] _clients;
        private CardCollectionManager _cardCollection;
        private BuffCardManager _buffCardManager;

        //private Dictionary<int, Dictionary<Guid, int>> _clientBattlefieldSlots = new Dictionary<int, Dictionary<Guid, int>>(); // get slots by holderUUId
        private Dictionary<Guid, BattleSlot> _clientBattlefieldSlots = new Dictionary<Guid, BattleSlot>(); // get slots by holderUUId
        public int _total_slots = 8;
        public BattlefieldManager(int id, int[] clients, CardCollectionManager cardCollectionM)
        {
            _cardCollection = cardCollectionM;
            _id = id;
            _clients = clients;
            _buffCardManager = new BuffCardManager(id, clients,_cardCollection);
            GenenrateBattlefieldSlots();
            Events.onCardDestroy.AddListener(OnCardDestroy);
            Events.onJoinGameScene.AddListener(OnJoinGameScene);
        }

        public BuffCardManager GetBuffCardManager()
        {
            return _buffCardManager;
        }

        void GenenrateBattlefieldSlots()
        {
            for(int i = 1; i < Server._maxPlayers+1; ++i)
            {
                for(int l = 0; l < _total_slots; ++l)
                {
                    Guid triggerUUID = Guid.NewGuid();
                    _clientBattlefieldSlots.Add(triggerUUID, new BattleSlot(triggerUUID, i, l));
                }
                //_clientBattlefieldSlots.Add(i, uuid_slot);
            }
        }
        void SendBattleSlots(int clientID)
        {
            int id = clientID;
            //if(clientID < 0)
            //{
            //    id = Server.GetOpponentForId(Math.Abs(clientID));
            //}
            //Dictionary<Guid, int> uuid_slots = _clientBattlefieldSlots[id];
            Guid[] uuids = new Guid[_total_slots];
            foreach (KeyValuePair<Guid, BattleSlot> entry in _clientBattlefieldSlots)
            {
                if (id == entry.Value._ownerID)
                    uuids[entry.Value._slot] = entry.Value._triggerUUID;
            }
            
            //if(clientID < 0)
            //{
            //    ServerSend.InitBattleSlots(Math.Abs(clientID), false, uuids); //TOD mieti
            //    return;
            //}
            ServerSend.InitBattleSlots(clientID, true, uuids);
            ServerSend.InitBattleSlots(Server.GetOpponentForId(clientID), false, uuids);
        }

        public BattleSlot GetBattleSlot(Guid triggerUUID)
        {
            if (_clientBattlefieldSlots.ContainsKey(triggerUUID))
                return _clientBattlefieldSlots[triggerUUID];
            return new BattleSlot(Guid.Empty,-1,-1);
        }

        void OnJoinGameScene(int clientID)
        {
            Console.WriteLine("SENDING battleslots!");
            SendBattleSlots(clientID);
            //SendBattleSlots(clientID*-1);
        }

        //public void SetClientBattleFieldSlot

        public void Battle(int attackerClientID, Guid attackerTriggerUUID, Guid victimTriggerUUID,Guid attacker_uuid, Guid victim_uuid)
        {
            Player attackerPlayer = Server._clients[attackerClientID]._player;
            Player opponent = Server._clients[Server.GetOpponentForId(attackerClientID)]._player;
            MonsterCard cVictim = (MonsterCard)opponent.GetBattlefield().GetCard(victim_uuid);
            if (cVictim == null)
                return;


            Card attCard = attackerPlayer.GetBattlefield().GetCard(attacker_uuid);
            if(attCard == null)
            {
                attCard = attackerPlayer.GetHand().GetCard(attacker_uuid);
                if (attCard != null && attCard is SpellCard)
                    Server._areaTriggerManager.OnPlaySpellCard(attackerPlayer, GetBattleSlot(victimTriggerUUID), attCard);
                return;
            }
            MonsterCard cAttacker =(MonsterCard)attCard;
            Console.WriteLine($"{cAttacker._name} => {cVictim._name}");
            if (cVictim == null || cAttacker == null)
            {
                Console.WriteLine($"No valid cards victims: {cVictim} or attackers: {cAttacker}");
                return;
            }
            
            if(!Server._clients[cVictim.GetOwner()]._player.GetBattlefield().CanCardBeAttacked(cVictim))
            {
                Console.WriteLine($"{cAttacker._name} couldn't attack");
                return;
            }

            if (!cAttacker.IsAbleToAttack())
            {
                return;
            }

            if (!actionPointReduce(attackerPlayer))
                return;
            
            cAttacker.TakeOrAddHealth(-cVictim._attack);
            cAttacker.AddAmountOfAttacks(-1);
            cVictim.TakeOrAddHealth(-cAttacker._attack);

            //Server._battlefieldManager.OnCardTakeDamage(cAttacker);
            //Server._battlefieldManager.OnCardTakeDamage(cVictim);

            UpdateCardOnBattleField(attackerClientID, cVictim.GetOwner(), cAttacker, cVictim);

        }
        bool actionPointReduce(Player player)
        {
            if (player.ReduceRoundActionPoints(Constants.ACTIONPOINT_COST_ATTACKING_WITH_CARD))
            {
                if (Constants.ACTIONPOINT_COST_ATTACKING_WITH_CARD > 0)
                    ServerSend.SendPlayerData(player._id);
                return true;
            }
            return false;
        }
        public int FindCardOwnerFromBattlefield(Guid uuid)
        {

            for(int i = 1; i < Server._maxPlayers+1; ++i)
            {
                if(Server._clients[i]._player != null )
                {
                    if (Server._clients[i]._player.GetBattlefield().GetCard(uuid) != null)
                    {

                        return i;
                    }
                }
            }
            return -1;
        }
        void OnCardDestroy(Card card)
        {
            Player player = Server._clients[card.GetOwner()]._player;
            player.GetBattlefield().RemoveCard(card);
            player.GetTable().GetDeck(Enums.DeckType.GRAVEYARD).AddCard(Server.GetCardCollectionManager().GetCard(card._id));
            //player.GetBattlefield().PrintBattleField();


        }
        //public void CheckIfDead(int clientID, Card card)
        //{
        //    if (card._defence == 0) //dead
        //    {
                
                
        //    }
        //}
        public void UpdateCardOnBattleField(int aClientID,int vClientID, Card aCard, Card vCard)
        {
            //CheckIfDead(aClientID, aCard);
            //CheckIfDead(vClientID, vCard);
            ServerSend.SendBattleCardUpdate(aClientID, aCard, vCard);
            ServerSend.SendBattleCardUpdate(vClientID, aCard, vCard);
        }

        public void HitHero(int clientID, Guid heroUUID, Guid cardUUID)
        {
            //Console.WriteLine("Hitting hero head..");
            Player aPlayer = Server._clients[clientID]._player; //attacker
            MonsterCard c = (MonsterCard) aPlayer.GetBattlefield().GetCard(cardUUID);
            if (c == null)
            {
                Console.WriteLine("HitHead Couldnt find card!");
                return;
            }
            Player vPlayer = Server._heroManager.FindPlayerHero(heroUUID); //victim
            
            if (vPlayer._id == aPlayer._id)
            {
                Console.WriteLine($"Client({clientID}) Try to hit his own head!");
                return;
            }
                

            if(vPlayer == null)
            {
                Console.WriteLine("Couldnt find hero from player");
                return;
            }

            if(!vPlayer.GetBattlefield().CanAttackHero())
            {
                Console.WriteLine("Cant hit Hero! There is cards front/backline");
                return;
            }

            Console.WriteLine($"Card({c._name}) => Hero({vPlayer.GetHero()._name})");

            if(!c.IsAbleToAttack())
            {               
                return;
            }

            if (!actionPointReduce(aPlayer))
                return;

            c.AddAmountOfAttacks(-1);

            bool endGame = false;
            if (vPlayer.GetHero().AddDamage(-c._attack)) endGame = true;

            Server._heroManager.SendHeroesToClients(clientID);
            
            if(endGame)
            {
                Console.WriteLine($"Hero({vPlayer.GetHero()._name}) hp is 0, game has lost for player{vPlayer._username}");
                Server.GetRoundManager(aPlayer._id).GameHasEnded(aPlayer._id);
            }

        }
    }
}
