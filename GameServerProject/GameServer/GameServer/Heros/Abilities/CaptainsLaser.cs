using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Heros.Passives;

namespace GameServer.Heros.Abilities
{
    [Serializable]
    public class CaptainsLaser : HeroAbility
    {
        int _damage = 1;
        public CaptainsLaser(Heroo hero) : base(hero)
        {
            _title = "Captain's laser";
            _description = "Deals 1 damage to enemies hero";
            _activePassive = _passives[0];
        }

        public override void InitPassives()
        {
            _passives[0] = new HPHeal("Damage covered to heal");
            _passives[1] = new HPExtraDmg("Deals 1 extra dmg",1);
            _passives[2] = new HPExtraDmg("Deals 2 extra dmg",2);
            
        }

        public override void Activate()
        {
            Console.WriteLine("Using captains laser boom boom");

        }
    }
}
