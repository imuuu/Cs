using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Heros;
using GameServer.Interfaces.Heros;
namespace GameServer.Heros
{
    [Serializable]
    public abstract class HeroAbility : IHeroAbility
    {
        protected Heroo _hero;
        protected string _description = "";
        protected string _title ="";
        protected HAbilityPassive[] _passives;
        protected HAbilityPassive _activePassive;
        
        public HeroAbility(Heroo hero)
        {
            _hero = hero;
            _passives = new HAbilityPassive[3];
            InitPassives();
        }
        public virtual void InitPassives(){}

        public string GetDescription()
        {
            return _description;
        }

        public string GetTitle()
        {
            return _title;
        }

        public void SetActivePassive(HAbilityPassive passive)
        {
            _activePassive = passive;
        }

        public HAbilityPassive GetPassive()
        {
            return _activePassive;
        }

        public void ActivatePassive()
        {
            _activePassive.Activate();
        }

        public virtual void Activate(){}

       
    }
}
