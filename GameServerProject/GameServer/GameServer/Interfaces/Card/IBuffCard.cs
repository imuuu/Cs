using GameServer.Enums;
using System;
using System.Collections.Generic;

namespace GameServer
{
    public interface IBuffCard
    {
        Dictionary<Guid, Buff> _buffs { get; set; }
        List<Buff> _setted_buffs { get; set; }
        public CardFaction _faction { get; set; }
        public CardType _type { get; set; }
        public int _amountOFAttacks { get; set; }


        public Card SetCardFaction(CardFaction faction);

        public CardFaction GetCardFaction();

        
        public Card SetCardType(CardType type);
        public CardType GetCardType();
        public Card AddBuff(Buff buff);
        public List<Buff> GetAllBuffs();
        public List<Buff> GetAllBuffsSameTriggerType(BuffTriggerType type);
        public List<Buff> GetAllSettedBuffs();
        public Buff GetSettedBuff(Guid uuid);
        public bool HasSettedBuffs();
        public void RemoveBuff(IBuff buff);
        public void SetAllSettedBuffs(List<Buff> buffs);
        public void SetBuff(Buff buff);

        public bool IsAbleToAttack();
        public void SetAmountOfAttacks(int amount);

        public void AddAmountOfAttacks(int amount);
        public int GetAmountOfAttacks(int amount);

        public void ResetAmountOfAttacks();
    }
}