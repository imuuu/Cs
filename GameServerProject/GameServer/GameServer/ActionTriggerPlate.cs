using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
namespace GameServer
{
    public class ActionTriggerPlate
    {
        public Card _card;
        public ActionTriggerType _triggerType;
        public Card _target_card;

        public ActionTriggerPlate(Card card, ActionTriggerType actionTrigger)
        {
            _card = card;
            _triggerType = actionTrigger;
        }

        public ActionTriggerPlate SetTarget(Card target)
        {
            _target_card = target;
            return this;
        }
    }
}
