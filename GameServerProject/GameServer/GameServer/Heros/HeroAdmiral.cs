using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Interfaces.Heros;
using GameServer.Heros.Abilities;
namespace GameServer.Heros
{
    [Serializable]
    class HeroAdmiral : Heroo
    {
        public HeroAdmiral(HeroEnum heroEnum) : base(heroEnum)
        { 
            _name = "Admiral";
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
            ////low cards
            //for(int i = 11; i < 21; ++i)
            //{
            //    AddDefaultCard(i, 2); //low
            //}
           
            

            ////mid cards
            //AddDefaultCard(1, 5); //mid
            //AddDefaultCard(10, 5); //mid
            //AddDefaultCard(20, 3); //mid
            //AddDefaultCard(36, 2); //mid
            //AddDefaultCard(25, 2); //mid


            ////high cards
            //AddDefaultCard(18, 10); //high;
            //AddDefaultCard(21, 10); //high;



        }
    }
}
