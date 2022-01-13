using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Interfaces.Card;
using GameServer.Buffs;
namespace GameServer
{
    public class SpellCard : Card, ISpellCard
    {
        Buff _buff_for_targers;
        public SpellCard(int id, string name, string desc, int manaCost) : base(id, name, desc, manaCost)
        {
            _classType = Enums.CardClassType.SPELL;
        }
        public SpellCard() { _classType = Enums.CardClassType.SPELL; }

        public SpellCard SetTargetBuff(Buff buff)
        {
            _buff_for_targers = buff;
            return this;
        }

        public virtual void ActivateSpell(ICard target)
        {
            GameInfo gInfo = Server._gameScene.GetGameInfo(_ownerClientID);
            Server._gameScene.GetBattlefieldManager(gInfo._id).GetBuffCardManager().BuffTypeSender((Card)target, _buff_for_targers);
            //Server._battlefieldManager.GetBuffCardManager().BuffTypeSender((Card)target, _buff_for_targers);
        }

        public override Card Clone(Card clone)
        {

            clone = base.Clone(clone);
            if(_buff_for_targers != null)
                ((SpellCard)clone)._buff_for_targers = _buff_for_targers.Clone();
            
            clone._classType = Enums.CardClassType.SPELL;
            return clone;
        }
        public override Card GetNewInstance()
        {
            return new SpellCard();
        }
    }
}
