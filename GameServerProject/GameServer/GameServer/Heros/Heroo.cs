using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Heros;
using GameServer.Interfaces.Heros;

namespace GameServer
{
    [Serializable]
    public abstract class Heroo : IHero
    {
        public HeroEnum _heroEnum;
        public Player _player;
        public string _name;
        public int _hp;
        public Guid _uuid;
        protected int _defHp;
        public HeroAbility _activeAbility;
        protected List<HeroAbility> _abilites;
        private Dictionary<int, int> _defCards;
        //protected List<Card> _cards;

        public Heroo(HeroEnum heroEnum)
        {
            _heroEnum = heroEnum;
            _uuid = Guid.NewGuid();
            _abilites = new List<HeroAbility>();
            _defCards = new Dictionary<int, int>();
            InitAbilities();
            InitDefaultCards();
        }
        public Heroo SetNewGuid()
        {
            _uuid = Guid.NewGuid();
            return this;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public List<HeroAbility> GetAbilities()
        {
            return _abilites;
        }

        public int IsDefaultCard(int id)
        {
            if (_defCards.ContainsKey(id)) return _defCards[id];
            return 0;
        }

        public void AddDefaultCard(int id, int amount)
        {
            _defCards[id] = amount;
        }
        public bool AddDamage(int amount)
        {
            _hp += amount;
            if (_hp > _defHp)
                _hp = _defHp;

            if (_hp <= 0)
            {
                _hp = 0;

                //TODO HERO IS DEAD!
                return true;
            }
            return false;

        }
        public abstract void InitDefaultCards();
        public virtual void InitAbilities()
        {
        }
        public void SetActiveAbility(HeroAbility ability)
        {
            _activeAbility = ability;
        }

        public void TriggerAbility()
        {
            _activeAbility.Activate();
        }

      
    }
}
