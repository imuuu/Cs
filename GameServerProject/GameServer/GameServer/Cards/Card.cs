using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using GameServer.Enums;
using GameServer.Interfaces.Card;
namespace GameServer
{
    public class Card : IEquatable<Card>, ICard, IActionTrigger
    {
        public int _battlefieldSlotID { get ; set ; }
        public string _description { get; set; }
        public int _id { get; set; }
        public string _name { get; set; }
        public int _ownerClientID { get; set; }
        public CardState _state { get; set; }
        public Guid _uuid { get; set; }
        public int _manaCost { get; set; }
        public CardClassType _classType { get; set; }
        public Dictionary<ActionTriggerType, List<Action>> _actions { get; set; }

        HeroEnum[] _belongs;
        //int id, string name, string desc, int attack, int def, int goldcost, int manacost, CardType type
        public Card(int id, string name, string desc, int manaCost)
        {
            _id = id;
            _name = name;
            _description = desc;          
            _uuid = Guid.NewGuid();
            _manaCost = manaCost;
            _classType = CardClassType.NONE;
            _actions = new Dictionary<ActionTriggerType, List<Action>>();
            //Events.onActionTrigger.AddListener(TriggerAction);
        }
        public Card()
        {
            _classType = CardClassType.NONE;
        }
        public virtual void PrintData()
        {
            Console.WriteLine($"id: {_id} " +
                              $"name: {_name} ");
                              //$"GoldCost: {_goldCost} "+
                              //$"Attack: {_attack} "+
                              //$"Health: {_defence} "+
                              //$": {1}");
        }
        public virtual void SetOwner(int clientID)
        {
            _ownerClientID = clientID;
        }
        public int GetOwner()
        {
            return _ownerClientID;
        }

        public void SetNewUUID()
        {
            _uuid = Guid.NewGuid();
        }
        
        public virtual Card Clone(Card clone)
        {
            //Console.WriteLine("Visited card: "+clone);
            //Card clone = new Card(_id,_name,_description, _manaCost);
            clone._name = _name;
            clone._id = _id;
            clone._description = _description;
            clone._manaCost = _manaCost;
            clone.SetNewUUID();
            //clone._faction = _faction;
            clone._state = _state;
            clone._battlefieldSlotID = _battlefieldSlotID;
            clone._ownerClientID = _ownerClientID;
            clone._classType = CardClassType.NONE;
            clone._actions = CloneActions();
            return clone;
        }

        public bool Equals(Card card)
        {
            if (card == null)
                return false;
            if(ReferenceEquals(this, card))
            {
                return this._uuid.Equals(card._uuid);
            }

            return false;
           
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }

        public static bool operator ==(Card c1, Card c2)
        {
            if(c1 is null)
            {
                if(c2 is null)
                {
                    return true;
                }
                return false;
            }

            return (c1.Equals(c2));
        }

        public static bool operator !=(Card c1, Card c2) => !(c1 == c2);

        public override int GetHashCode()
        {
            return _id;
        }

        private Dictionary<ActionTriggerType, List<Action>> CloneActions()
        {
            Dictionary<ActionTriggerType, List<Action>> dic = new Dictionary<ActionTriggerType, List<Action>>();
            foreach (KeyValuePair<ActionTriggerType, List<Action>> entry in _actions)
            {
                dic.Add(entry.Key, entry.Value);
            }
            return dic;
        }

        public void AddActionTrigger(ActionTriggerType triggerType, Action action)
        {
            if (_actions.ContainsKey(triggerType))
            {
                _actions[triggerType].Add(action);
                return;
            }
            _actions[triggerType] = new List<Action>() { action };
        }

        public List<Action> GetActions(ActionTriggerType triggerType)
        {
            if (_actions.ContainsKey(triggerType))
                return _actions[triggerType];
            return null;
        }

        public virtual void TriggerAction(ActionTriggerPlate triggerPlate)
        {
            if (!triggerPlate._card.Equals(this))
                return;

            List<Action> actions = GetActions(triggerPlate._triggerType);
            if (actions == null)
                return;
            foreach (Action action in actions)
            {
                action();
            }
        }

        public virtual Card GetNewInstance()
        {
            return new Card();
        }

        public Card BelongsTo(HeroEnum heroEnum)
        {
            BelongsTo(new HeroEnum[] { heroEnum });
            return this;
        }

        public Card BelongsTo(HeroEnum[] heroEnum)
        {
            _belongs = heroEnum;
            return this;
        }

        public bool HasHerosCard(Heroo hero)
        {
            if (_belongs == null) return true;

            foreach(HeroEnum he in _belongs)
            {
                if (hero._heroEnum == he) return true;
            }

            return false;
        }
    }
}
