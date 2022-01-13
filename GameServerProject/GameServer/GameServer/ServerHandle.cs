using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Enums;
using GameServer.Interfaces.Card;
namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();

            Console.WriteLine($"{Server._clients[fromClient]._tcp._socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}");
            if(fromClient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }

            //Server._clients[fromClient].SendIntoGame(username);

            //if (clientIdCheck == Constants.TOTAL_PLAYERS_TO_START)
            //{
            //    //Server._roundManager.SetPlayerTurn(1);
            //    Server._roundManager.SetStarted();
            //    //Server._roundManager.RoundStarts(Server._roundManager.GetPlayerWhosTurn());
            //    Server._heroManager.SendHeroesToClients();

            //}
        }
        /// <summary>
        /// Client triggered when clicked specific deck
        /// </summary>
        /// <param name="fromClient"></param>
        /// <param name="packet"></param>
        public static void DeckClick(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            int deckId = packet.ReadInt();
            DeckBuyType deckBuyType = (DeckBuyType)packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            if(!Server.GetRoundManager(fromClient).isPlayerTurn(fromClient))
            {
                return;
            }

            if (IsIndicatorEnabled(fromClient))
                return;

            //Console.WriteLine($"Player (ID: {fromClient}) has clicked deck:  ({deckId})!");

            Player player = Server._clients[fromClient]._player;
            
            Table table = player.GetTable();
            Deck deck = table.GetDeck((DeckType)deckId);

            Server._deckManager.DeckPurchase(player, deck, deckBuyType);
            
        }

        public static bool IsIndicatorEnabled(int clientid)
        {
            if (Server.GetBattlefieldManager(clientid).GetBuffCardManager().IsClientMiddleOfChosenFromIndicatorCards(clientid))
            {
                Console.WriteLine("Need to chose from indicator cards");

                return true;
            }
            return false;
        }
       
        public  static void OnAreaTableTriggerDrop(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            if (fromClient != clientIdCheck)
                return;

            if (!Server.GetRoundManager(fromClient).isPlayerTurn(fromClient))
            {
                return;
            }

            Server._areaTriggerManager.OnAreaTriggerDrop(Server._clients[clientIdCheck]._player, 
                packet.ReadGuid(), 
                packet.ReadInt(), 
                packet.ReadGuid());
        }
        public static void PlayerHitHead(int fromClient, Packet packet)
        {
            //Console.WriteLine("Receiving hitting player head from client");
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            if (!Server.GetRoundManager(fromClient).isPlayerTurn(fromClient))
            {
                return;
            }

            if (IsIndicatorEnabled(fromClient))
                return;

            Guid heroUUID = packet.ReadGuid();
            Guid cardUUID = packet.ReadGuid();
            Server.GetBattlefieldManager(fromClient).HitHero(fromClient, heroUUID, cardUUID);
        }

       

        public static void OnEnterCardHolder(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            if (!Server.GetRoundManager(fromClient).isPlayerTurn(fromClient))
            {
                return;
            }

            int holderID = packet.ReadInt();
            CardHolderEnterType hType = (CardHolderEnterType)packet.ReadInt();
            Guid card_uuid = packet.ReadGuid();

            switch (hType)
            {
                case CardHolderEnterType.SHOW_INDICATORS:
                    Player player = Server._clients[clientIdCheck]._player;
                    Card c = player.GetHand().GetCard(card_uuid);
                    //Console.WriteLine("show indicators: 1");
                    if (!(c is IMonsterCard))
                        return;

                    //Console.WriteLine("show indicators: 2");

                    if (c == null || !((MonsterCard)c).HasSettedBuffs() || !player.GetBattlefield().IsValidSlot(holderID))
                        return;

                    //Console.WriteLine("show indicators: 3");

                    c = c.Clone(new MonsterCard());
                    c._battlefieldSlotID = holderID;
                    List<Buff> buffs = ((MonsterCard)c).GetAllBuffsSameTriggerType(BuffTriggerType.ON_PLAY);
                    if (buffs.Count == 1)
                    {
                        GameInfo gInfo = Server._gameScene.GetGameInfo(c.GetOwner());

                        List<Card> cards = Server._gameScene.GetBattlefieldManager(gInfo._id).GetBuffCardManager().GetCardsOfBuffPattern(c, buffs[0]);
                        ServerSend.SendCardIndicators(c.GetOwner(), cards.Select(u => u._uuid).ToArray(), -1, holderID);
                        return;
                    }
                    
                    break;
            }
        }
        public static void ReceivedChosenIndicatorCard(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            if (!Server.GetRoundManager(fromClient).isPlayerTurn(fromClient))
            {
                return;
            }

            List<Guid> card_uuids = new List<Guid>();
            int total_cards = packet.ReadInt();
            //Console.WriteLine("Loading cards: ");
            for (int i = 0; i < total_cards; ++i)
            {
                card_uuids.Add(packet.ReadGuid());
            }
            Server.GetBattlefieldManager(fromClient).GetBuffCardManager().ChosenCard(fromClient, card_uuids);
        }

        /// <summary>
        /// Reads chosen card from client! Will be changed in future.. for debug!
        /// </summary>
        /// <param name="fromClient"></param>
        /// <param name="packet"></param>
        public static void PlayerDecks(int fromClient, Packet packet)
        {
            //Console.WriteLine("PlayerDecks");
            //int[] all_decks = new int[5 * 20];
            //int clientIdCheck = packet.ReadInt();

            //if (fromClient != clientIdCheck)
            //    return;

            //for (int i = 0; i < all_decks.Length; i++)
            //{
            //    int id = packet.ReadInt();
            //    all_decks[i] = id;
            //}
            //Server._clients[fromClient]._player.GetTable().InitDecks(all_decks);

            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;
            int[] cardIDs = new int[packet.ReadInt()];
            for(int i = 0; i < cardIDs.Length; i++)
            {
                cardIDs[i] = packet.ReadInt();
            }
            Server._clients[fromClient]._player.GetTable().InitDecks(cardIDs);

        }

        public static void RecevingUsername(int fromClient, Packet packet)
        {
            if (fromClient != packet.ReadInt()) return;

            string username = packet.ReadString();
            Server._clients[fromClient]._player._username = username;
        }

        public static void SceneLoaded(int fromClient, Packet packet)
        {
            if (fromClient != packet.ReadInt()) return;
            Scene_enum scene = (Scene_enum)packet.ReadInt();
            if(scene ==Scene_enum.GAME_SCENE)
            {
                Server.GetRoundManager(fromClient).StartTheGame(fromClient);
            }
        }

        public static void JoinAndLeaveQueue(int fromClient, Packet packet)
        {
            if (fromClient != packet.ReadInt()) return;

            if(packet.ReadBool()) // join
            {
                Server._lobby.JoinQueue(fromClient);
                return;
            }
            Server._lobby.LeaveQueue(fromClient);
        }

        public static void ReceivingAskHeroCards(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;
            int heroID = packet.ReadInt();
            Heroo hero = Server._heroManager.GetHero((HeroEnum)heroID);
            //Console.WriteLine($"Client {fromClient} chose hero: {hero._name} with uudi: {hero._uuid}");
            Server._clients[fromClient]._player.SetHero(hero);
           
            ServerSend.SendCollectionCards(fromClient, hero);
        }

        public static void ReceivingChosenCollectionCards(int fromClient, Packet packet)
        {
            Console.WriteLine("Receive chosen collec");
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            int[] cardIDs = new int[packet.ReadInt()];
            for (int i = 0; i < cardIDs.Length; i++)
            {
                cardIDs[i] = packet.ReadInt();
            }
            Server._clients[fromClient]._player.GetTable().InitDecks(cardIDs);
        }

        /// <summary>
        /// Triggered when choses card from deck purcase, normally 3 options to chose from
        /// </summary>
        /// <param name="fromClient"></param>
        /// <param name="packet"></param>
        public static void ChosenCard(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            int deckID = packet.ReadInt();
            Guid uuid = Guid.Parse(packet.ReadString());
            Player player = Server._clients[fromClient]._player;
            Server._deckManager.PutChosenCardToOwnDeck(player, uuid, player.GetTable().GetDeck((DeckType)deckID));

        }

       
        /// <summary>
        /// Triggered when player attacks with his card to other card ( happens in battlefield )
        /// </summary>
        /// <param name="fromClient"></param>
        /// <param name="packet"></param>
        public static void ReceivedBattleBetweenCards(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;
            
            if (!Server.GetRoundManager(fromClient).isPlayerTurn(fromClient))
            {
                return;
            }

            if (IsIndicatorEnabled(fromClient))
                return;

            Console.WriteLine("Battle received from client: "+ clientIdCheck);

            Guid attackerTriggerUUID = Guid.Parse(packet.ReadString());
            Guid victimTriggerUUID = Guid.Parse(packet.ReadString());
            Guid attackerUUID = Guid.Parse(packet.ReadString());
            Guid victimUUID = Guid.Parse(packet.ReadString());
            Server.GetBattlefieldManager(fromClient).Battle(fromClient, attackerTriggerUUID, victimTriggerUUID,attackerUUID, victimUUID);
        }

        /// <summary>
        /// Test purpose, Can be included if valid.. For Sending small packets without extra work
        /// </summary>
        /// <param name="fromClient"></param>
        /// <param name="packet"></param>
        public static void ReceivedMiniPacket(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;



            if (IsIndicatorEnabled(fromClient))
                return;

            MiniPackets miniPacket = (MiniPackets)packet.ReadInt();
            int code = packet.ReadInt();

            switch (miniPacket)
            {
                //case MiniPackets.END_TURN:
                //{
                //        if(code == 0)
                //        {
                //            RoundState state = Server._roundManager.SwitchState();
                //            if(state == RoundState.PURCHACE)
                //            {
                //                //END turn;
                //                Server._roundManager.SwitchNextPlayer();
                //            }
                //            else
                //            {

                //            }
                //        }
                //        break;
                //}
                case MiniPackets.ROUND_STATE:
                {
                        if(Server.GetRoundManager(clientIdCheck).GetPlayerWhosTurn() == clientIdCheck)
                        {
                            Server.GetRoundManager(clientIdCheck).ButtonPressEndTurn();
                        }
                        
                        break;
                }
            }
        }

        public static void OnClassAbilityTrigger(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();

            if (fromClient != clientIdCheck)
                return;

            Server._clients[clientIdCheck]._player.GetHero().TriggerAbility();

        }
        
    }
}