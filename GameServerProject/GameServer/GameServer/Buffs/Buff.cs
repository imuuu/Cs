using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
using GameServer;
namespace GameServer
{

    public class Buff : IBuff
    {
        public Guid _uuid { get; set; }
        public Pattern _pattern { get; set; }
        public BuffState _state { get; set; }
        public BuffType _type { get; set; }
        public BuffTriggerType _trigger { get; set; }
        public bool _include_self { get; set; }
        public CardFaction _buffingFaction { get; set; }
        public CardType _buffingCardType { get; set; }
        public int _value { get; set; }

        /// <summary>
        /// adsd
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pattern"></param>
        /// <param name="state"></param>
        /// <param name="trigger"></param>
        /// <param name="include_self"></param>
        /// <param name="value"></param>
        public Buff(BuffType type, Pattern pattern, BuffState state, BuffTriggerType trigger, bool include_self, int value)
        {
            _type = type;
            _pattern = pattern;
            _state = state;
            _trigger = trigger;
            _value = value;
            _include_self = include_self;
            _uuid = Guid.NewGuid();
            _buffingCardType = CardType.NONE;
            _buffingFaction = CardFaction.NONE;

        }

        public Buff() { }

        public Buff SetNewUUID()
        {
            _uuid = Guid.NewGuid();
            return this;
        }

        public virtual Buff Clone()
        {
            Buff clone = new Buff(_type, _pattern, _state,_trigger, _include_self,_value);
            return clone;
        }

        public Buff SetFaction(CardFaction cardFaction)
        {
            _buffingFaction = cardFaction;
            return this;
        }

        public Buff SetCardType(CardType cardType)
        {
            _buffingCardType = cardType;
            return this;
        }

        //public virtual Buff MakeThisDebuff()
        //{
        //    _value *= -1;
        //    return this;
        //}
    }

      

       
       
    
}
