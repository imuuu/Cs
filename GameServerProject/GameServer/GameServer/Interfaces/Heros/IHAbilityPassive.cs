using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Interfaces.Heros
{
    interface IHAbilityPassive
    {
        void Activate();
        public string GetDescription();
    }
}
