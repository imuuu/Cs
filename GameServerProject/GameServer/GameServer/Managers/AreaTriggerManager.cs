using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
namespace GameServer.Managers
{
    public class AreaTriggerManager
    {
        public void OnAreaTriggerDrop(Player player, Guid triggerUUID, int holderID, Guid card_uuid)
        {
            if (ServerHandle.IsIndicatorEnabled(player._id))
                return;
            BattleSlot battleSlot = Server._gameScene.GetBattlefieldManager(Server._gameScene.GetGameInfo(player._id)._id).GetBattleSlot(triggerUUID);
            Card card = player.GetHand().GetCard(card_uuid);
            
            
            if (card == null)
            {
                Console.WriteLine($"Couldnt find a card from player {player._username}");
                return;
            }
            if (card._manaCost > player.GetMana())
            {
                Console.WriteLine($"Player {player._username} doesnt have enough mana");
                return;
            }
            if (!player.isEnoughActionPoints(Constants.ACTIONPOINT_COST_PLAYING_CARD_ON_TABLE))
            {
                Console.WriteLine($"Player {player._username} doesnt have enough action points");
                return;
            }

            if (card is SpellCard)
            {
               
                OnPlaySpellCard(player, battleSlot, card);                
                return;
            }

            if (holderID < 0)
            {
                if (card is VictoryCard)
                {
                    //Events.onActionTrigger.Invoke(new ActionTriggerPlate(card, Enums.ActionTriggerType.ON_PLAY));
                    OnPlayVictoryCard(player, triggerUUID, card);
                    Events.onCardPlay.Invoke(card);
                    return;
                }
            }

            if (battleSlot._ownerID != player._id)
            {
                Console.WriteLine($"{player._username} Try to play card wrong holder!");
                return;
            }
           

            Console.WriteLine("area trigger:");

            card.TriggerAction(new ActionTriggerPlate(card, ActionTriggerType.ON_PLAY));

            if (player.GetBattlefield().isHolder(holderID) && (card is IMonsterCard))
            {
                //Events.onActionTrigger.Invoke(new ActionTriggerPlate(card, Enums.ActionTriggerType.ON_PLAY));
                OnDroppingCardHolder(player, triggerUUID, holderID, card);
                Events.onCardPlay.Invoke(card);
                
                return;
            }

            player.ReduceRoundActionPoints(Constants.ACTIONPOINT_COST_PLAYING_CARD_ON_TABLE);
            

            if (card is SpellCard)
            {
                Console.WriteLine("Try to play spell card!");
            }

        }
        void OnDroppingCardHolder(Player player, Guid triggerUUID, int holderID, Card card)
        {
            if (!player.GetBattlefield().AddCard(holderID, card))
            {
                Console.WriteLine($"Player {player._username} couldn't add card to battlefield?");
                return;
            }
            player.ReduceRoundActionPoints(Constants.ACTIONPOINT_COST_PLAYING_CARD_ON_TABLE);
            
            player.GetHand().RemoveCard(card);
            player.GetHand().PrintCards();
            player.AddMana(card._manaCost * -1);

            ServerSend.SendPlayerData(player._id);
            //ServerSend.SendCardToHolder(player._id, holderID, player.GetBattlefield().GetHolderType(holderID), card);
            ServerSend.SendAreaTriggerResponse(player._id, Enums.AreaTriggerSendCode.SET_Card, triggerUUID, card);
            //ServerSend.SendCardToHolderMirror(player._id * -1, holderID, player.GetBattlefield().GetHolderType(holderID), card);           
            ServerSend.SendCardToHolderMirror(Server.GetOpponentForId(player._id), holderID, player.GetBattlefield().GetHolderType(holderID), card);           
        }

        void OnPlayVictoryCard(Player player, Guid triggerUUID, Card card)
        {
            //player.AddMoneyIncome(((VictoryCard)card)._incomeIncrease);
            
            player.GetHand().RemoveCard(card);
            player.AddMana(card._manaCost * -1);
            ((VictoryCard)card).TriggerAction(new ActionTriggerPlate(card, ActionTriggerType.ON_PLAY));
            ServerSend.SendPlayerData(player._id);
            //ServerSend.SendAreaTriggerResponse(player._id, Enums.AreaTriggerSendCode.NO_Card_And_Destroy, triggerUUID, card);
        }

        public void OnPlaySpellCard(Player player, BattleSlot battleSlot, Card card)
        {
            //card.TriggerAction(new ActionTriggerPlate(card, ActionTriggerType.ON_PLAY));
            Console.WriteLine("Its a spell");
            SpellCard spellcard = (SpellCard)card;
            Card victimCard = battleSlot.GetCard();

            if (victimCard == null)
            {
                Console.WriteLine($"====> but victimCard not found from player{battleSlot._ownerID}");
                return;
            }

            Console.WriteLine($"====> hit {victimCard._name}");
            spellcard.ActivateSpell(victimCard);
            
            player.GetHand().RemoveCard(card);
            //ServerSend.RemoveCardFromHand(player._id, card);
            //ServerSend.SendAreaTriggerResponse(player._id, Enums.AreaTriggerSendCode.NO_Card_And_Destroy, battleSlot._triggerUUID, card);
        }

    }
}
