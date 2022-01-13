using GameServer.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager _instance;

    public static Dictionary<int, PlayerManager> _players = new Dictionary<int, PlayerManager>();
    public Dictionary<Guid, AreaTrigger> _areaTriggers = new Dictionary<Guid, AreaTrigger>();

    public GameObject _localPlayerPrefab;
    public GameObject _playerPrefab;
    public Scene_enum _state = Scene_enum.StartMenu;
    private CardBuilder _cardBuilder;
    private Hand _handScript;
    private Player _player;
    private CanvasManager _canvasManager;
    private BuffCardManager _buffCardManager;
    private HeroCollection _heroCollection;
    private int[] _cardIDs;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Instance already exists, destroy object!");
            Destroy(this);
        }

        _cardBuilder = FindObjectOfType<CardBuilder>();
       
        _heroCollection = new HeroCollection();
        _buffCardManager = GetComponent<BuffCardManager>();

    }
    private void OnLevelWasLoaded(int level)
    {
        print("level loaded from manager");
        _players = new Dictionary<int, PlayerManager>();
        _areaTriggers = new Dictionary<Guid, AreaTrigger>();

        if (_state == Scene_enum.GameScene)
        {
            _handScript = FindObjectOfType<Hand>();
            _canvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
           
        }
    }
    public void SetCardIDs(int[] cardIDS)
    {
        _cardIDs = cardIDS;
    }
    
    public int[] GetCardIds()
    {
        return _cardIDs;
    }

    public void RegisterAreaTrigger(AreaTrigger areaTrigger)
    {
        if (_areaTriggers.ContainsKey(areaTrigger.GetUUID()))
            _areaTriggers.Remove(areaTrigger.GetUUID());

        _areaTriggers.Add(areaTrigger.GetUUID(), areaTrigger);
    }
    public AreaTrigger GetAreaTrigger(Guid uuid)
    {
        if (_areaTriggers.ContainsKey(uuid))
            return _areaTriggers[uuid];
        return null;
    }
    public BuffCardManager GetBuffCardManager()
    {
        return _buffCardManager;
    }

    public HeroCollection GetHeroCollection()
    {
        return _heroCollection;
    }

    public Player GetPlayer()
    {
        return _player;
    }

    public Hand GetHand()
    {
        return _handScript;
    }
    public void SpawnPlayer(PlayerData playerData)
    {
        GameObject player;
        PlayerManager pManager;
        //playerData.PrintData();
        if (playerData._id == Client._instance._myId)
        {
            player = Instantiate(_localPlayerPrefab, Vector3.zero, Quaternion.identity);
            pManager = player.GetComponent<PlayerManager>();
            pManager.SetPlayerData(playerData); 
            player.GetComponent<Player>().SetHandSize(playerData._handSize); // MIGHT GIVE ERROR
            _player = player.GetComponent<Player>();
        }
        else
        {
            player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            pManager = player.GetComponent<PlayerManager>();
            pManager.SetOpponetUsername(playerData._username);
        }


        //Debug.Log("made error sometimes ? add id: " + playerData._id);
        _players.Add(playerData._id, player.GetComponent<PlayerManager>());
        //playerData.PrintData();
    }

    public void RefreshPlayerData(PlayerData playerData)
    {

        if (!_players.ContainsKey(playerData._id))
            return;

        PlayerManager pManager = _players[playerData._id];
        pManager.SetPlayerData(playerData);
        Events.onRefreshPlayerData.Invoke();
    }

    public void RemovePlayerFromPlayers(int id)
    {
        _players.Remove(id);
    }

    public void SpawnCardToHand(int clientID, Card card)
    {
        if(card._id == 0)
        {
            //card is null;
            Debug.Log("Deck is empty");
            return;
        }
        if(clientID == Client._instance._myId)
        {
            _player.AddCardToHand(card);
        }
    }
    //public void SpawnCardToHandOpponent(int totalCards)
    //{
    //    //NOT WORKING
    //    GameObject[] cards = new GameObject[totalCards];
    //    for(int i = 0; i < totalCards; ++i)
    //    {
    //        GameObject spawned_card = _cardBuilder.SpawnCard();
    //        spawned_card.GetComponent<CardUI>().SetAsCover();
    //        cards[i] = spawned_card;
    //    }
    //    _handScript.UpdateHandUIOpponent(cards, totalCards);
    //}

    public void SpawnCardToHolder(int holderID, CardHolderType holderType, Card card)
    {
        foreach(TableCardHolder holder in FindObjectsOfType<TableCardHolder>())
        {
            if(holder.GetHolderType() == holderType && holder.GetId() == holderID)
            {
               
                //if(holder.GetHolderType() == CardHolderType.FRONTLINE || holder.GetHolderType() == CardHolderType.BACKLINE)
                //{
                //    holder.DestroyOnHoldDropObject(true);
                //}
                if(holder.GetCard() != null)
                {
                    holder.UpdateCard(card);
                    return;
                }
                GameObject cardObj = _cardBuilder.SpawnCard(card, this);
                cardObj.GetComponent<CardUI>().SetTableHolder(holder.gameObject);
                holder.SetCard(cardObj);
                return;
            }
        }
    }

    public void SpawnCardToHolder(Guid triggerUUID, Card card)
    {
        TableCardHolder holder = (TableCardHolder)GetAreaTrigger(triggerUUID);
        //GetAreaTrigger(triggerUUID).DestroyOnHoldDropObject(true);
        if (holder.GetCard() != null)
        {
            holder.UpdateCard(card);
            return;
        }
        GameObject cardObj = _cardBuilder.SpawnCard(card, this);
        cardObj.GetComponent<CardUI>().SetTableHolder(holder.gameObject);
        holder.SetCard(cardObj);
        UICanvasManager._instance.DrainMana(card._manaCost);
        return;
    }

    public void InitHero(int id, Hero hero)
    {
        foreach(HeroUI hUi in GameObject.FindObjectsOfType<HeroUI>())
        {
            if(id == hUi.id)
            {
                hUi.SetData(hero, id);
            }
        }
       
    }
    //public void InitHeroAbility(Guid heroUUID, string title, string description, string passiveTitle, string passiveDesc)
    //{
    //    foreach (HeroUI hUi in GameObject.FindObjectsOfType<HeroUI>())
    //    {
    //        if (hUi.GetHero()._uuid.Equals(heroUUID))
    //        {
    //            hUi.
    //        }
    //    }

    //}
    //public void InitHeroAbilityPassive(int id, Hero hero)
    //{
    //    foreach (HeroUI hUi in GameObject.FindObjectsOfType<HeroUI>())
    //    {
    //        if (id == hUi.id)
    //        {
    //            hUi.SetData(hero);
    //        }
    //    }

    //}

    public GameObject UpdateCardFromHolder(Card card)
    {
        GameObject o = FindCardGameObjectFromHolder(card);
        o.GetComponent<CardUI>().SetCardUIData(card);
        return o;
    }
    public GameObject FindCardGameObjectFromHolder(Card card)
    {
        foreach (TableCardHolder holder in FindObjectsOfType<TableCardHolder>())
        {
            Card c = holder.GetCard();
            if (c != null && c._uuid.Equals(card._uuid))
            {
                return holder.GetCardGameobject();
            }

        }
        return null;
    }

    public void UpdateBattleBetweenCards(Card aCard, Card vCard)
    {
        GameObject aCardO = FindCardGameObjectFromHolder(aCard);
        GameObject vCardO = FindCardGameObjectFromHolder(vCard);

        Task.Delay(300).ContinueWith(x =>
        {
            aCardO.GetComponent<CardUI>().SetCardUIData(aCard);
            vCardO.GetComponent<CardUI>().SetCardUIData(vCard);
        });

        AnimationManager._instance.PlayBattleHitAnimation(aCardO, vCardO,aCard,vCard);
    }

    public void UpdateBattlefieldCard(Card card)
    {
        GameObject aCardO = FindCardGameObjectFromHolder(card);
        aCardO.GetComponent<CardUI>().SetCardUIData(card);
        AnimationManager._instance.CheckDeath(aCardO, card);
        if (_buffCardManager.isIndicatorsEnabled())
        {
            _buffCardManager.ResetIndicators();
        }
        //Task.Delay(300).ContinueWith(x =>
        //{
        //    aCardO.GetComponent<CardUI>().setCardUIData(card);
        //});
           
   
    }
    public void Minipacket(Packet packet)
    {
        MiniPackets miniPacket = (MiniPackets)packet.ReadInt();
        int code = packet.ReadInt();
        switch (miniPacket)
        {
            case MiniPackets.END_TURN:
                {
                    int roundTime = packet.ReadInt();
                    if (code == 0)
                    {
                        _canvasManager._roundManager.ShowTextInfoPanel("Your Turn Has Ended!");
                        _canvasManager._roundManager.SetRoundTime(0);
                        //_canvasManager._roundManager.SetRoundTime(0);
                        _canvasManager._roundManager.ShowStateButton(false);
                        break;
                    }
                    if(code == 1)
                    {
                        _canvasManager._roundManager.ShowTextInfoPanel("Your Turn Has Started!");
                        _canvasManager._roundManager.SetRoundTime(roundTime);
                        //_canvasManager._roundManager.ShowRoundTime(true); //TODO might give error
                        _canvasManager._roundManager.ShowStateButton(true);
                        UICanvasManager._instance.ReFillMana();
                    }
                    break;
                }
            case MiniPackets.ROUND_STATE:
                {
                    RoundState rState = (RoundState)code;
                    _canvasManager._roundManager.SetState(rState);
                    break;
                }
            case MiniPackets.GAME_START:
                {
                    int data = packet.ReadInt();
                    _canvasManager._roundManager.ShowTextInfoPanel("Game Starts! "+data);
                    break;
                }
            case MiniPackets.GAME_END:
                {
                    string username = packet.ReadString();
                    if(code == 1) _canvasManager._roundManager.ShowTextInfoPanel("You have won against " + username+"!");
                    if(code == 0) _canvasManager._roundManager.ShowTextInfoPanel("You have lost against " + username+ "!");

                    break;
                }
        }
    }
    public void DisplayChosenCards(int deckId, Card[] cards)
    {
        _canvasManager._choseCardTab.InitShowCardsUI(deckId, cards);
    }

    public void RefreshDecks()
    {
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Deck"))
        {
            TableDeck deck = o.GetComponent<TableDeck>();
            deck.SetCostOfBuyType(deck._cost_gold, DeckBuyType.GOLD);
            deck.SetCostOfBuyType(deck._cost_mana, DeckBuyType.MANA);
            deck.SetCostOfBuyType(deck._cost_health, DeckBuyType.HEALTH);
            deck.SetCostOfBuyType(deck._cost_actionPoint, DeckBuyType.ACTION_POINT);
        }
    }

    public void ShowIndicators(Guid[] card_uuids, int total_choses)
    {
        _buffCardManager.SetTotalChoses(total_choses);
        _buffCardManager.ShowInducators(card_uuids, Color.green);
    }

    public void InitBattleSlots(bool own, Guid[] battleSlotIDs)
    {
        Debug.Log("OWN: " + own);
        foreach(TableCardHolder tch in GameObject.FindObjectsOfType<TableCardHolder>())
        {
            if(own)
            {
                if(tch.GetHolderType() == CardHolderType.FRONTLINE || tch.GetHolderType() == CardHolderType.BACKLINE)
                {
                    tch.Register(battleSlotIDs[tch.GetId()]);
                }
                continue;
            }
            else
            {
                if (tch.GetHolderType() == CardHolderType.MIRROR_FRONTLINE || tch.GetHolderType() == CardHolderType.MIRROR_BACKLINE)
                {
                    tch.Register(battleSlotIDs[tch.GetId()]);
                }
                continue;
            }
        }
    }

   
}
