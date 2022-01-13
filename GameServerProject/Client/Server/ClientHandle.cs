using GameServer.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int myId = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client._instance._myId = myId;
        //Client._instance.SetConnected(true);

        ClientSend.WelcomeReceived();
        //ClientSend.SendPlayerDecks();
        
        Client._instance._udp.Connect(((IPEndPoint)Client._instance._tcp._socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet packet)
    {
        GameManager._instance.SpawnPlayer(packet.ReadPlayerData());
    }

    public static void AddCardToHand(Packet packet)
    {
       
        int clientId = packet.ReadInt();
        Card card = packet.ReadCard();
        
        GameManager._instance.SpawnCardToHand(clientId, card);
        
    }

    public static void AddCardToHandOpponent(Packet packet)
    {
        int totalCards = packet.ReadInt();
        //GameManager._instance.SpawnCardToHandOpponent(totalCards);
    }

    //public static void PutCardToHolder(Packet packet)
    //{
    //    Card card = packet.ReadCard();

    //    int holderID = packet.ReadInt();
    //    CardHolderType hType = (CardHolderType)packet.ReadInt();
    //    GameManager._instance.SpawnCardToHolder(holderID, hType, card);
    //}
    public static void PutCardToHolderMirror(Packet packet)
    {
        Card card = packet.ReadCard();

        int holderID = packet.ReadInt();
        CardHolderType hType = (CardHolderType)packet.ReadInt();
        if(hType == CardHolderType.FRONTLINE)
        {
            hType = CardHolderType.MIRROR_FRONTLINE;
        }else
        {
            hType = CardHolderType.MIRROR_BACKLINE;
        }
        GameManager._instance.SpawnCardToHolder(holderID, hType, card);
    }

    public static void RefreshPlayerData(Packet packet)
    {
        //int id = packet.ReadInt();
        //string username = packet.ReadString();
        //int money = packet.ReadInt();
        //int mana = packet.ReadInt();
        //int moneyIncome = packet.ReadInt();
        //int manaCap = packet.ReadInt();
        GameManager._instance.RefreshPlayerData((PlayerData)packet.ReadPlayerData());
        GameManager._instance.RefreshDecks();
    }

    public static void InitDeck(Packet packet)
    {
        int deckID = packet.ReadInt();
        foreach(TableDeck tDeck in FindObjectsOfType<TableDeck>())
        {
            if(tDeck._id == deckID)
            {
                tDeck.SetCostOfBuyType(packet.ReadInt(), DeckBuyType.GOLD);
                tDeck.SetCostOfBuyType(packet.ReadInt(), DeckBuyType.MANA);
                tDeck.SetCostOfBuyType(packet.ReadInt(), DeckBuyType.HEALTH);
                tDeck.SetCostOfBuyType(packet.ReadInt(), DeckBuyType.ACTION_POINT);
                tDeck.SetTotalCards(packet.ReadInt());
                return;
            }
        }
    }

    public static void ChoseCardFromDeck(Packet packet)
    {
        int deckID = packet.ReadInt();
        int cardsCount = packet.ReadInt();
        Card[] cards = new Card[cardsCount];
        for(int i = 0; i < cardsCount; ++i)
        {
            Card card = packet.ReadCard();
            cards[i] = card;
        }

        GameManager._instance.DisplayChosenCards(deckID, cards);
    }

    public static void ReceivedMiniPacket(Packet packet)
    {
        GameManager._instance.Minipacket(packet);
    }

    public static void UpdateBattleCard(Packet packet)
    {
        Card attCard = packet.ReadCard();
        Card victimCard = packet.ReadCard();
        GameManager._instance.UpdateBattleBetweenCards(attCard, victimCard);

    }

    public static void UpdateBattlefieldCards(Packet packet)
    {
       
        Card[] cards = new Card[packet.ReadInt()];
        //Debug.Log("Update battelfield cards total: "+cards.Length);
        for (int i = 0; i < cards.Length; ++i)
        {
            GameManager._instance.UpdateBattlefieldCard(packet.ReadCard());
        }
    }

    public static void InitHero(Packet packet)
    {
        int id = packet.ReadInt();
        Hero hero = packet.ReadHero();
        GameManager._instance.InitHero(id, hero);
    }

    public static void ReceiveCardIndicators(Packet packet)
    {
        int askingHolderID = packet.ReadInt();
        int total_choses = packet.ReadInt();
        Guid[] card_guids = new Guid[packet.ReadInt()];
        for(int i = 0; i < card_guids.Length; ++i)
        {
            card_guids[i] = packet.ReadGuid();
        }
        //Debug.Log($"Received indicators totalChoses: {total_choses}, askingHolderID: {askingHolderID}, arrayL: {card_guids.Length}");

        if(total_choses == -1 && askingHolderID > -1)
        {
            if(GameManager._instance.GetBuffCardManager().GetAskingHolder() == askingHolderID)
            {
                GameManager._instance.GetBuffCardManager().ShowInducators(card_guids, Color.blue);
            }

            return;
        }
        GameManager._instance.GetBuffCardManager().ResetIndicators();
        GameManager._instance.ShowIndicators(card_guids, total_choses);
    }


    /// <summary>
    /// Triggers when player try to drop card to some kind of trigger on battle field normally cardholder
    /// </summary>
    /// <param name="packet"></param>
    public static void ResponseAreaTrigger(Packet packet)
    {
        Guid triggerUUID = packet.ReadGuid();
        AreaTriggerSendCode code = (AreaTriggerSendCode)packet.ReadInt();
        Card card;
        switch (code)
        {
            case AreaTriggerSendCode.NO_Card:
                break;
            case AreaTriggerSendCode.NO_Card_And_Destroy:
                //Debug.Log($"try to find this: {GameManager._instance.GetAreaTrigger(triggerUUID)}, uuid: {triggerUUID}");
                //GameManager._instance.GetAreaTrigger(triggerUUID).DestroyOnHoldDropObject(true);
                break;
            case AreaTriggerSendCode.SET_Card:
                card = packet.ReadCard();
                GameManager._instance.SpawnCardToHolder(triggerUUID, card);
                break;
        }

        
    }

    public static void InitBattleSlots(Packet packet)
    {
        bool own = packet.ReadBool();
        int lenght = packet.ReadInt();
        Guid[] guids = new Guid[lenght];
        for(int i = 0; i < guids.Length; ++i)
        {
            guids[i] = packet.ReadGuid();
        }
        GameManager._instance.InitBattleSlots(own, guids);
    }

    public static void RemoveCardFromHand(Packet packet)
    {
        Guid cardUUID = packet.ReadGuid();
        Destroy( GameManager._instance.GetPlayer().removeCardFromHand(cardUUID));
    }

    public static void InitClassAbility(Packet packet)
    {
        
    }

    public static void InitClassAbilityPassive(Packet packet)
    {
        
    }

    public static void InitHeroesToCollection(Packet packet)
    {
        Hero[] heros = new Hero[packet.ReadInt()];
        for(int i = 0; i < heros.Length; ++i)
        {
            heros[i] = packet.ReadHero();
        }
        GameManager._instance.GetHeroCollection().InitHeros(heros);
    }

    internal static void ReceiveHeroColCards(Packet packet)
    {
        //print("receving hero cards from server");
        Card[] cards = new Card[packet.ReadInt()];
        Dictionary<int, int> counts = new Dictionary<int, int>();
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i] = packet.ReadCard();
            counts[cards[i]._id] = packet.ReadInt();
            //print($"card: {cards[i]._name} : {cards[i]._classType}");
        }
        GameObject.FindObjectOfType<CardCollectionMenu>().SetCards(cards, counts);
    }

    public static void ReceiveSceneLoad(Packet packet)
    {
        Scene_enum scene = (Scene_enum)packet.ReadInt();
        GameManager._instance._state = scene;
        if(scene == Scene_enum.GameScene)
        {
            UIManager._instance.StopWaitingForOpponent();
        }
        
        LevelManager._instance.LoadScene(scene);
    }
}
