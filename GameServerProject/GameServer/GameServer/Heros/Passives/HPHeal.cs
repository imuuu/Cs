using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Heros.Passives
{
    [Serializable]
    class HPHeal : HAbilityPassive
    {
        
        public HPHeal(string desc) : base(desc)
        {
           
        }

        public override void Activate()
        {
            base.Activate();
        }
    }
}
