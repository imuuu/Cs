using System;
using GameServer.Enums;
namespace GameServer
{
    public interface IBuff
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
        Buff Clone();
    }

      

       
       
    
}
