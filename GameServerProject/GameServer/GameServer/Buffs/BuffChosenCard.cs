using GameServer.Enums;
namespace GameServer
{
    public class BuffChosenCard : Buff , IBuffChosenCard
    {
        public int _total_choses { get; set; }

        public BuffChosenCard(int totalChoses,BuffType type, Pattern pattern, BuffState state, BuffTriggerType trigger, bool include_self, int value) : base(type, pattern, state, trigger, include_self, value)
        {
            //this._buffingCardType = buffingType;
            //this._buffingFaction = buffingFaction;
            this._total_choses = totalChoses;
        }

        
        public override Buff Clone()
        {
            return new BuffChosenCard(_total_choses,_type, _pattern, _state, _trigger, _include_self,_value);
        }
    }

      

       
       
    
}
