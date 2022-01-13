using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
using System.Linq;
namespace GameServer.Cards
{
    public class BuffCard : Card, IBuffCard
    {
        public Dictionary<Guid, Buff> _buffs { get; set; }
        public List<Buff> _setted_buffs { get; set; }
        public CardFaction _faction { get; set; }
        public CardType _type { get; set; }

        public int _amountOFAttacks { get; set; }
        int _defAmountOFAttacks = 0;

        public BuffCard(int id, string name, string desc, int manaCost) : base(id, name, desc, manaCost)
        {
            _buffs = new Dictionary<Guid, Buff>();
            _setted_buffs = new List<Buff>();
            SetAmountOfAttacks(1);
        }

        public BuffCard()
        {

        }
        public override Card GetNewInstance()
        {
            return new BuffCard();
        }
        public void SetBuff(Buff buff)
        {
            _setted_buffs.Add(buff);
        }

        public List<Buff> GetAllSettedBuffs()
        {
            return _setted_buffs;
        }

        public void SetAllSettedBuffs(List<Buff> buffs)
        {
            _setted_buffs = buffs;
        }

        public bool HasSettedBuffs()
        {
            return _setted_buffs.Count > 0;
        }

        public Buff GetSettedBuff(Guid uuid)
        {
            return _setted_buffs.FirstOrDefault(x => x._uuid == uuid);
        }

        public List<Buff> GetAllBuffsSameTriggerType(BuffTriggerType type)
        {
            List<Buff> bs = new List<Buff>();
            foreach (Buff b in _setted_buffs)
            {
                if (b._trigger == type)
                    bs.Add(b);
            }
            return bs;
        }

        public Card AddBuff(Buff buff)
        {
            _buffs[buff._uuid] = buff;
            return this;
        }

        public void RemoveBuff(IBuff buff)
        {
            _buffs.Remove(buff._uuid);
        }

        public List<Buff> GetAllBuffs()
        {
            return _buffs.Values.ToList();
        }

        private void SettedBuffClone(BuffCard buffCard)
        {
            List<Buff> buffss = new List<Buff>();
            foreach (Buff b in _setted_buffs)
            {
                buffss.Add(b.Clone().SetNewUUID());
            }
            buffCard.SetAllSettedBuffs(buffss);
        }

        public override Card Clone(Card clone)
        {
            //Console.WriteLine("Visited Buff: " + clone);
            clone = base.Clone(clone);

            //BuffCard bCard = new BuffCard(_id, _name, _description, _manaCost);
            ((BuffCard)clone).SetCardType(_type);
            ((BuffCard)clone).SetCardFaction(_faction);
            
            ((BuffCard)clone)._buffs = new Dictionary<Guid, Buff>();
            ((BuffCard)clone)._setted_buffs = new List<Buff>();
            SettedBuffClone(((BuffCard)clone));
            ((BuffCard)clone).SetAmountOfAttacks(_amountOFAttacks);

            //clone._classType = CardClassType.NONE;

            return clone;
        }

        public CardFaction GetCardFaction()
        {
            return _faction;
        }

        public Card SetCardFaction(CardFaction faction)
        {
            _faction = faction;
            return this;
        }


        public Card SetCardType(CardType type)
        {
            _type = type;
            return this;
        }

        public CardType GetCardType()
        {
            return _type;
        }

        public bool IsAbleToAttack()
        {
            if (_amountOFAttacks > 0)
            {
                return true;
            }
            Console.WriteLine($"Card({_name}) doesnt have more attacks!");
            return false;
        }

        public void SetAmountOfAttacks(int amount)
        {
            _amountOFAttacks = amount;
            _defAmountOFAttacks = amount;
        }

        public int GetAmountOfAttacks(int amount)
        {
            return _amountOFAttacks;
        }

        public void ResetAmountOfAttacks()
        {
            _amountOFAttacks = _defAmountOFAttacks;
        }

        public void AddAmountOfAttacks(int amount)
        {
            _amountOFAttacks += amount;
        }
    }
}
