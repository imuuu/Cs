using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Interfaces.Heros;

namespace GameServer.Heros
{
    [Serializable]
    public abstract class HAbilityPassive : IHAbilityPassive
    {
        string _description = "";
        string _title = "Passive";

        public HAbilityPassive(string description)
        {
            _description = description;
        }

        public virtual void Activate() { }

        public string GetDescription()
        {
            return _description;
        }

        public string GetTitle()
        {
            return _title;
        }
    }
}
