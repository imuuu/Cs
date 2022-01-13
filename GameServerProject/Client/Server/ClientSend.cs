using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client._instance._tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client._instance._udp.SendData(packet);
    }

    #region
    public static void WelcomeReceived()
    {
        using(Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {  
            packet.Write(Client._instance._myId);
            packet.Write("");
            SendTCPData(packet);
        }
    }

    public static void SendPlayerDecks(int[] cardIds)
    {
        //TODO l‰hett‰‰ oma pakka yms t‰h‰
        //using(Packet packet = new Packet((int)ClientPackets.playerDecks))
        //{
        //    PlayerDecks playerDecks = FindObjectOfType<PlayerDecks>();
        //    packet.Write(Client._instance._myId);
        //    int[] all_decks = playerDecks.GetALLDeckCardsArray();
        //    for(int i = 0; i < all_decks.Length; ++i)
        //    {
        //        packet.Write(all_decks[i]);

        //    }
        //    SendTCPData(packet);
        //}
        
        using (Packet packet = new Packet((int)ClientPackets.playerDecks))
        {
            packet.Write(Client._instance._myId);
            packet.Write(cardIds.Length);
            foreach(int cardID in cardIds)
            {
                packet.Write(cardID);
            }
            SendTCPData(packet);
        }
    }

    public static void HitHero(Guid uuid, Card cAttacker)
    {
        using(Packet packet = new Packet((int)ClientPackets.hitHero))
        {
            packet.Write(Client._instance._myId);
            packet.Write(uuid);
            packet.Write(cAttacker._uuid);
            SendTCPData(packet);
        }
    }

    internal static void TriggerClassAbility()
    {
        using (Packet packet = new Packet((int)ClientPackets.OnTriggerClassAbility))
        {
            packet.Write(Client._instance._myId);
            SendTCPData(packet);
        }
    }

    public static void DeckClicked(int deckID, DeckBuyType buyType)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerClickedDeck))
        {
            packet.Write(Client._instance._myId);
            packet.Write(deckID);
            packet.Write((int)buyType);
            SendTCPData(packet);
        }
    }

    public static void AskChosenHeroCards(int heroId)
    {
        using (Packet packet = new Packet((int)ClientPackets.AskChosenHeroCards))
        {
            packet.Write(Client._instance._myId);
            packet.Write(heroId);
            //packet.Write((int)buyType);
            SendTCPData(packet);
        }
    }

    //public static void DeckDropCardToHolder(TableCardHolder holder, Card card)
    //{
    //    using (Packet packet = new Packet((int)ClientPackets.playerDropCardToHolder))
    //    {
    //        packet.Write(Client._instance._myId);
    //        packet.Write(holder.GetId());
    //        packet.Write((int)holder.GetHolderType());
    //        packet.Write(card._uuid.ToString());
    //        //Debug.Log("Card has been drop to holder: id: " + holderID);
    //        SendTCPData(packet);
    //    }
    //}

    public static void OnAreaTableTriggerDrop(AreaTriggerDrop areaTriggerDrop)
    {
        using (Packet packet = new Packet((int)ClientPackets.OnAreaTableTriggerDrop))
        {

            packet.Write(Client._instance._myId);
            packet.Write(areaTriggerDrop._uuidAreaTrigger);
            packet.Write(areaTriggerDrop._holderID);
            packet.Write(areaTriggerDrop._card._uuid);
            SendTCPData(packet);
        }
    }

    public static void SendChosenCard(int deckId, string uuid)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerChosenCard))
        {
            packet.Write(Client._instance._myId);
            packet.Write(deckId);
            packet.Write(uuid);
            SendTCPData(packet);
        }
    }

    public static void SendMiniPacket(MiniPackets miniPacket, int code)
    {
        using (Packet packet = new Packet((int)ClientPackets.miniPackets))
        {
            packet.Write(Client._instance._myId);
            packet.Write((int)miniPacket);
            packet.Write((int)code);
            SendTCPData(packet);
        }
    }

    public static void SendChosenIndicatorCard(List<Guid> uuids)
    {
        using (Packet packet = new Packet((int)ClientPackets.SendChosenIndicatorCard))
        {
            packet.Write(Client._instance._myId);
            packet.Write(uuids.Count);
            foreach(Guid uuid in uuids)
            {
                packet.Write(uuid);
            }      
            SendTCPData(packet);
        }
    }

    public static void SendBattleBetweenCards(Guid attacker_triggerUUID, Guid victim_triggerUUID, Guid attacker, Guid victim)
    {
        using (Packet packet = new Packet((int)ClientPackets.battleBetweenCards))
        {
            packet.Write(Client._instance._myId);
            packet.Write(attacker_triggerUUID);
            packet.Write(victim_triggerUUID);
            packet.Write(attacker.ToString());
            packet.Write(victim.ToString());
            SendTCPData(packet);
        }
    }

    public static void OnEnterCardHolder(int holderID, Guid card_uuid, CardHolderEnterType type)
    {
        using (Packet packet = new Packet((int)ClientPackets.OnEnterCardHolder))
        {
            packet.Write(Client._instance._myId);
            packet.Write(holderID);
            packet.Write((int)type);
            packet.Write(card_uuid);
            SendTCPData(packet);
        }
    }

    public static void OnJoinAndLeaveQueue(bool join)
    {
        using (Packet packet = new Packet((int)ClientPackets.JoinAndLeaveQueue))
        {
            packet.Write(Client._instance._myId);
            packet.Write(join);
            SendTCPData(packet);
        }
    }

    public static void SceneLoaded(Scene_enum sceneEnum)
    {
        using(Packet packet = new Packet((int)ClientPackets.SceneLoaded))
        {
            packet.Write(Client._instance._myId);
            packet.Write((int)sceneEnum);
            SendTCPData(packet);
        }
    }

    public static void SendUsername(string username)
    {
        using (Packet packet = new Packet((int)ClientPackets.SendUsername))
        {
            packet.Write(username);
            SendTCPData(packet);
        }
    }
    #endregion
}
