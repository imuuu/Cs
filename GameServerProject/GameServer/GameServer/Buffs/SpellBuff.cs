using GameServer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Buffs
{
    public class SpellBuff : Buff
    {
        public SpellBuff(BuffType type, Pattern pattern, int value)
        {
            _uuid = Guid.NewGuid();
            _buffingCardType = CardType.NONE;
            _buffingFaction = CardFaction.NONE;
            _state = BuffState.TEMP;
            _include_self = true;
            _trigger = BuffTriggerType.NONE;
            _value = value;
            _type = type;
            _pattern = pattern;
        }
        public override Buff Clone()
        {
            return new SpellBuff(_type, _pattern, _value);
        }
    }
}
