namespace GameServer
{
    //b.GetType() == typeof(A)
    //public class BuffFaction : Buff
    //{
    //    public CardFaction _buffingFaction;
    //    public BuffFaction(BuffType type, Pattern pattern, BuffState state, CardFaction buffingFaction, BuffTriggerType trigger, bool include_self, int value) : base(type, pattern, state, trigger, include_self, value)
    //    {
    //        this._buffingFaction = buffingFaction;
    //    }

    //    public override Buff Clone()
    //    {
    //        Buff clone = new BuffFaction(_type, _pattern, _state, _buffingFaction, _trigger, _include_self, _value);
    //        return clone;
    //    }
    //}

    //public class BuffCardType : Buff
    //{
    //    public CardType _buffingCardType;

    //    public BuffCardType(BuffType type, Pattern pattern, BuffState state, CardType buffingType, BuffTriggerType trigger, bool include_self, int value) : base(type, pattern, state, trigger, include_self, value)
    //    {
    //        this._buffingCardType = buffingType;
    //    }

    //    public override Buff Clone()
    //    {
    //        Buff clone = new BuffCardType(_type, _pattern, _state, _buffingCardType, _trigger, _include_self, _value);
    //        return clone;
    //    }
    //}

    interface IBuffChosenCard
    {
        int _total_choses { get; set; }
    }

      

       
       
    
}
