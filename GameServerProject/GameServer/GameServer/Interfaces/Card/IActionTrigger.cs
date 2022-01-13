using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
namespace GameServer.Interfaces.Card
{
    public interface IActionTrigger
    {
        public Dictionary<ActionTriggerType, List<Action>> _actions { get; set; }

        public void AddActionTrigger(ActionTriggerType triggerType, Action action);

        public List<Action> GetActions(ActionTriggerType triggerType);

        public void TriggerAction(ActionTriggerPlate triggerPlate);
    }
}
