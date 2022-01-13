using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Cards;
using GameServer.Enums;
namespace GameServer
{
    public class MonsterCard : BuffCard, IMonsterCard
    {
        public int _attack { get; set; }
        public int _defence { get; set; }
        public MonsterCard(int id, string name, string desc,int attack, int def, int manacost) : base(id, name, desc, manacost)
        {
            this._attack = attack;
            this._defence = def;
            _classType = CardClassType.MINION;

        }
        public MonsterCard()
        {
            _classType = CardClassType.MINION;
        }

        public override Card GetNewInstance()
        {
            return new MonsterCard();
        }
        public override Card Clone(Card clone)
        {
            //Console.WriteLine("Monster card: " + clone);

            clone = base.Clone(clone);
            ((MonsterCard)clone)._attack = _attack;
            ((MonsterCard)clone)._defence = _defence;
            clone._classType = CardClassType.MINION;
            //Console.WriteLine("Monster2 " + clone._name);
            //MonsterCard monsterCard = new MonsterCard(_id, _name, _description, _attack, _defence, _manaCost);
            //((BuffCard)clone).SetCardFaction(_faction);
            //((BuffCard)clone).SetCardType(_type);

            //List<Buff> buffss = new List<Buff>();
            //foreach (Buff b in _setted_buffs)
            //{
            //    //Console.WriteLine("Buff found: " + b._type.ToString());
            //    buffss.Add(b.Clone().SetNewUUID());
            //}
            //monsterCard.SetAllSettedBuffs(buffss);

            //SettedBuffClone((BuffCard)monsterCard);
            return clone;
        }
        public int TakeOrAddHealth(int amount)
        {
            _defence += amount;

            if (amount < 0)
                Events.onCardTakeDamage.Invoke(this);

            if (amount >= 0)
                Events.onCardHeal.Invoke(this);

            if (_defence <= 0)
            {
                _defence = 0;
                Console.WriteLine($"Card: {this._name} has died");
                Events.onCardDestroy.Invoke(this);
            }


            return _defence;
        }

        public int TakeOrAddAttack(int amount)
        {
            _attack += amount;
            if (_attack < 0)
                _attack = 0;
            return _attack;
        }

     

        
    }
}
