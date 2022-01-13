using GameServer.Enums;
namespace GameServer
{
    public class BuffChosenRoundCard : Buff, IBuffChosenCard, IBuffRound
    {
        public int _total_sustain_rounds { get; set; }
        public bool _keep_after_buffer_dies { get; set; }

        public int _total_choses { get; set; }
        public BuffChosenRoundCard(int totalChoses, int sustain_rounds, bool keep_after_buffer_dies, BuffType type, Pattern pattern, BuffState state, BuffTriggerType trigger, bool include_self, int value) : base(type, pattern, state, trigger, include_self, value)
        {
            this._total_choses = totalChoses;
            this._total_sustain_rounds = sustain_rounds;
            this._keep_after_buffer_dies = keep_after_buffer_dies;
        }

        public override Buff Clone()
        {
            return new BuffChosenRoundCard(_total_choses,_total_sustain_rounds, _keep_after_buffer_dies,_type, _pattern, _state, _trigger, _include_self, _value);
        }
    }

      

       
       
    
}
