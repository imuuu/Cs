using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
using GameServer.Interfaces.Card;
namespace GameServer
{
    public class VictoryCard : Card, IVictoryCard, IActionTrigger
    {
        private int _givenActionPoints = 0;
        private int _givenGold = 0;
      
        public VictoryCard(int id, string name, string desc, int manaCost, int givenActionPoints, int givenGold) : base(id, name, desc, manaCost)
        {

            _givenActionPoints = givenActionPoints;
            _givenGold = givenGold;

            _classType = Enums.CardClassType.VICTORY;
           
            
        }
        public VictoryCard()
        {
            _classType = Enums.CardClassType.VICTORY;
        }

        public override void SetOwner(int clientID)
        {
            base.SetOwner(clientID);
            AddActionTrigger(ActionTriggerType.ON_PLAY, () => Server._clients[_ownerClientID]._player.AddRoundActionPoints(_givenActionPoints + 1));
            AddActionTrigger(ActionTriggerType.ON_PLAY, () => Server._clients[_ownerClientID]._player.AddMoney(_givenGold));
        }

        public override Card GetNewInstance()
        {
            return new VictoryCard();
        }

        public override Card Clone(Card clone)
        {
            //Console.WriteLine("Victor card: " + clone);
            //VictoryCard vCard = new VictoryCard(_id, _name, _description, _manaCost, _givenActionPoints,_givenGold);
            clone = base.Clone(clone);
            ((VictoryCard)clone)._givenActionPoints = _givenActionPoints;
            ((VictoryCard)clone)._givenGold = _givenGold;
            clone._classType = CardClassType.VICTORY;
            return clone;
        }

        
    }
}
