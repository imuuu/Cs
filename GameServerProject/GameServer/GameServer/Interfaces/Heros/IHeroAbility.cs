using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Heros;
namespace GameServer.Interfaces.Heros
{
    interface IHeroAbility
    {
        public string GetDescription();

        public void SetActivePassive(HAbilityPassive passive);

        public void ActivatePassive();

        public void Activate();

        public void InitPassives();
    }
}
