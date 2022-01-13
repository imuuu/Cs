using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Interfaces.Heros;
using GameServer.Heros.Abilities;
namespace GameServer.Heros
{
    [Serializable]
    class HeroCaptain : Heroo
    {
        public HeroCaptain(HeroEnum heroEnum) : base(heroEnum)
        { 
            _name = "Captain";
            _hp = 30;
            _defHp = _hp;
            SetActiveAbility(_abilites[0]);
        }
        
        public override void InitAbilities()
        {
            _abilites.Add(new CaptainsLaser(this));
        }

        public override void InitDefaultCards()
        {
            //low cards
            for(int i = 21; i < 25; ++i)
            {
                AddDefaultCard(i, 4); //low
            }
            AddDefaultCard(25, 2);
            AddDefaultCard(26, 2);



            //mid cards
            for (int i = 27; i < 31; ++i)
            {
                AddDefaultCard(i, 4); //low
            }
            AddDefaultCard(31, 2);
            AddDefaultCard(32, 2);


            //high cards
            for (int i = 33; i < 37; ++i)
            {
                AddDefaultCard(i, 4); //low
            }
            AddDefaultCard(37, 2);
            AddDefaultCard(38, 2);



        }
    }
}
