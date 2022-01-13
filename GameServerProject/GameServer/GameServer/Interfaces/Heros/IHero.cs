using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Heros;

namespace GameServer
{ 
    public interface IHero
    {
        public void InitAbilities();
        public void SetActiveAbility(HeroAbility ability);

        public void TriggerAbility();

        //public Heroo GetNewInstance();
    }
}
