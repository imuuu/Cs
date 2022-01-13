using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Heros.Passives
{
    [Serializable]
    class HPExtraDmg : HAbilityPassive
    {
        int _extraDamage = 0;
        public HPExtraDmg(string description, int extraDamage) : base(description)
        {
            _extraDamage = extraDamage;
        }
    }
}
