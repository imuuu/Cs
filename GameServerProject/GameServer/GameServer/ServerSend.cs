using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
using GameServer.Heros;
namespace GameServer
{
    class ServerSend
    {
        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server._clients[toClient]._tcp.SendData(packet);
        }

        private static void SendUDPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server._clients[toClient]._udp.SendData(packet);
        }

        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for(int i = 1; i <= Server._maxPlayers; i++)
            {
                
                Server._clients[i]._tcp.SendData(packet);
            }
        }

        //public static void SendTCPDataSorter(int client_code, Packet packet)
        //{
        //    if (client_code == 0)
        //    {
        //        SendTCPDataToAll(packet);
        //        return;
        //    }

        //    if (client_code < 0)
        //    {
        //        SendTCPDataToAll(Math.Abs(client_code), packet);
        //        return;
        //    }
        //    SendTCPData(client_code, packet);
        //}

        private static void SendTCPDataToAll(int expectClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server._maxPlayers; i++)
            {
                if (i != expectClient)
                {
                    Server._clients[i]._tcp.SendData(packet);
                }                    
            }
        }

        private static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server._maxPlayers; i++)
            {
                Server._clients[i]._udp.SendData(packet);
            }
        }

        

        private static void SendUDPDataToAll(int expectClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server._maxPlayers; i++)
            {
                if (i != expectClient)
                {
                    Server._clients[i]._udp.SendData(packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int toClient, String msg)
        {
            using(Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(msg);
                packet.Write(toClient);
                SendTCPData(toClient, packet);
            }
        }

        public static void SpawnPlayer(int toClient, Player player)
        {
            using(Packet packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                packet.Write(player);
                SendTCPData(toClient, packet);
            }
        }

        public static void AddCardToHand(int toClient, Card card)
        {
            using(Packet packet = new Packet((int)ServerPackets.cardToHand))
            {
                packet.Write(Server._clients[toClient]._id);
                packet.Write(card);

                SendTCPData(toClient, packet);
            }

            using (Packet packet = new Packet((int)ServerPackets.cardToHandOpponent))
            {
                packet.Write(Server._clients[toClient]._player.GetHand().TotalCards());
                SendTCPDataToAll(toClient, packet);
            }

        }

        public static void SendAreaTriggerResponse(int clientID, AreaTriggerSendCode code ,Guid trigger_guid, Card card)
        {
            using (Packet packet = new Packet((int)ServerPackets.responseAreaTrigger))
            {
                packet.Write(trigger_guid);
                packet.Write((int)code);
                if(code == AreaTriggerSendCode.SET_Card)
                    packet.Write(card);

                SendTCPData(clientID, packet);
            }
        }

        public static void SendCollectionCards(int clientID, Heroo hero)
        {
            using (Packet packet = new Packet((int)ServerPackets.SendCollectionCards))
            {
                Card[] cards = Server._cardColManager.GetHerosCards(hero);
                packet.Write(cards.Length);
               
                foreach (Card card in cards) { packet.Write(card); packet.Write(hero.IsDefaultCard(card._id)); }
                SendTCPData(clientID, packet);
            }
        }

        public static void SendHeroes(int clientID)
        {
            using (Packet packet = new Packet((int)ServerPackets.initHeroes))
            {
                Heroo[] heros = Server._heroManager.GetHeros();
                packet.Write(heros.Length);
                for(int i = 0; i < heros.Length; i++)
                {
                    packet.Write(heros[i]);
                }

                SendTCPData(clientID, packet);
            }
        }

        //public static void SendCardToHolder(int toClient, int holderID, CardHolderType type, Card card)
        //{
        //    using (Packet packet = new Packet((int)ServerPackets.playerSendDroppedCardToHolder))
        //    {
        //        packet.Write(card);

        //        packet.Write(holderID);
        //        packet.Write((int)type);
        //        //packet.Write(Server._clients[toClient]._player.GetHand().TotalCards());

        //        Console.WriteLine("Card has been send to the holder");
        //        SendTCPData(toClient, packet);
        //    }


        //}
        public static void SendCardToHolderMirror(int toClient, int holderID, CardHolderType type, Card card)
        {
            using (Packet packet = new Packet((int)ServerPackets.sendDroppedCardOtherPlayers))
            {
                packet.Write(card);

                packet.Write(holderID);
                packet.Write((int)type);

                SendTCPData(toClient, packet);
                //SendTCPDataSorter(toClient, packet);
            }
        }
        public static void SendPlayerData(int toClient)
        {
            using (Packet packet = new Packet((int)ServerPackets.refreshPlayerData))
            {

                packet.Write(Server._clients[toClient]._player);
                SendTCPData(toClient, packet);
            }
        }

        public static void SendDeckData(int clientID, Deck deck)
        {
            using (Packet packet = new Packet((int)ServerPackets.initDeck))
            {
                packet.Write(deck.GetId());
                packet.Write(deck.GetCost(DeckBuyType.GOLD));
                packet.Write(deck.GetCost(DeckBuyType.MANA));
                packet.Write(deck.GetCost(DeckBuyType.HEALTH));
                packet.Write(deck.GetCost(DeckBuyType.ACTION_POINT));
                packet.Write(deck.GetCards().Count);
                //SendTCPDataToAll(packet);
                SendTCPData(clientID, packet);
            }
        }

        public static void SendCardChoses(int toClient, int deckId,List<Card> cards)
        {
            using (Packet packet = new Packet((int)ServerPackets.choseCardFromDeck))
            {
                packet.Write(deckId);
                packet.Write(cards.Count);
                for(int i = 0; i < cards.Count; ++i)
                {
                    packet.Write(cards[i]);
                }
                SendTCPData(toClient, packet);
            }
        }

        public static void SendBattleCardUpdate(int clientID, Card attack_card, Card victim_card)
        {
            using(Packet packet = new Packet((int)ServerPackets.updateBattleCard))
            {
                packet.Write(attack_card);
                packet.Write(victim_card);
                SendTCPData(clientID, packet);
            }
        }
        public static void SendBattlefieldCardUpdate(int clientCode, Card[] cards)
        {
            if(cards.Length <= 0)
            {
                Console.WriteLine("There aren't any cards to update");
            }
            else
            {
                using (Packet packet = new Packet((int)ServerPackets.updateBattlefieldCards))
                {
                    packet.Write(cards.Length);
                    for (int i = 0; i < cards.Length; ++i)
                    {
                        cards[i].PrintData();
                        packet.Write(cards[i]);
                    }
                    SendTCPData(clientCode, packet);
                    //SendTCPDataSorter(clientCode, packet);

                }
            }

            //Server._battlefieldManager.GetBuffCardManager().TriggerBuffOnHold(Server._roundManager.GetPlayerWhosTurn());
            Client client = Server._clients[clientCode];
            int gamesceneid = Server._gameScene.GetGameInfo(clientCode)._id;
            Server._gameScene.GetBattlefieldManager(gamesceneid).GetBuffCardManager().TriggerBuffOnHold(Server._gameScene.GetRoundManager(client).GetPlayerWhosTurn());


        }


        public static void InitHero(int clientID, Heroo hero, int id)
        {
            using (Packet packet = new Packet((int)ServerPackets.initHero))
            {
                packet.Write(id);
                packet.Write(hero);
                SendTCPData(clientID, packet);
                
                //SendTCPDataSorter(clientID, packet);
            }
        }

        public static void InitHeroAbility(int clientID, HeroAbility hAbility, Guid heroUUID)
        {
            using (Packet packet = new Packet((int)ServerPackets.initClassAbility))
            {
                packet.Write(heroUUID);
                packet.Write(hAbility.GetTitle());
                packet.Write(hAbility.GetDescription());
                SendTCPData(clientID,packet);
            }
        }
        public static void InitHeroAbilityPassive(int clientID, HAbilityPassive hAbilityPassive, Guid heroUUID)
        {
            using (Packet packet = new Packet((int)ServerPackets.initClassAbilityPassive))
            {
                packet.Write(heroUUID);
                packet.Write(hAbilityPassive.GetDescription());
                SendTCPData(clientID, packet);
            }
        }

        public static void SendMiniPacket<T>(int toClient, MiniPackets miniPacket, int code, T data)
        {
            using (Packet packet = new Packet((int)ServerPackets.miniPackets))
            {
                packet.Write((int)miniPacket);
                packet.Write((int)code);
                packet.Write(data);

                if(toClient == 0)
                {
                    SendTCPDataToAll(packet);
                    return;
                }

                if(toClient < 0)
                {
                    SendTCPDataToAll(Math.Abs(toClient), packet);
                    return;
                }
                SendTCPData(toClient, packet);
            }
        }

        /// <summary>
        /// Sending indicator patterns as cards for client
        /// </summary>
        /// <param name="client_code"> Send data to {< 0 = all execept client abs(<0)}, {0 = all clients}, { > 0 = only for that client}/param>
        /// <param name="card_uuids"></param>
        /// <param name="total_choses">how many choses client has, if none use -1</param>
        /// <param name="askedHolderID">default -1, only used if asking pattern from holder!</param>
        public static void SendCardIndicators(int client_code, Guid[] card_uuids, int total_choses, int askedHolderID)
        {
            if(card_uuids.Length <= 0)
            {
                //Console.WriteLine("None indicators found!");
                return;
            }
            using (Packet packet = new Packet((int)ServerPackets.sendCardIndicators))
            {
                packet.Write(askedHolderID);
                packet.Write(total_choses);
                packet.Write(card_uuids.Length);
                foreach(Guid uuid in card_uuids)
                {
                    packet.Write(uuid);
                }
                //Console.WriteLine("Sending indicators");
                //SendTCPDataSorter(client_code, packet);
                SendTCPData(client_code, packet);
            }
        }


        public static void InitBattleSlots(int clientID, bool own, Guid[] uuids)
        {

            using (Packet packet = new Packet((int)ServerPackets.initBattleSlots))
            {
                packet.Write(own);
                packet.Write(uuids.Length);
                for(int i = 0; i < uuids.Length; ++i)
                {
                    packet.Write(uuids[i]);
                }
                SendTCPData(clientID, packet);
            }
        }

        public static void RemoveCardFromHand(int clientID, Card card)
        {
            using (Packet packet = new Packet((int)ServerPackets.removeCardFromHand))
            {
                //Console.WriteLine($"Sending remove card from hand client: {clientID} and card {card._name}");
                packet.Write(card._uuid);               
                SendTCPData(clientID, packet);
            }
        }

        public static void SendToScene(int clientID, Scene_enum scene)
        {
            using(Packet packet = new Packet((int)ServerPackets.SendToScene))
            {
                packet.Write((int)scene);
                SendTCPData(clientID, packet);
            }
        }
        #endregion
    }
}
