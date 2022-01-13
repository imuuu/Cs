using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
namespace GameServer.Cards.Spells
{
    public class FireSpell : SpellCard
    {
        int _damage = 0;
        public FireSpell(int id, string name, string desc, int manaCost, int damage) : base(id, name, desc, manaCost)
        {
            _damage = damage;
            SetTargetBuff(new Buff(BuffType.GIVE_OR_TAKE_HEALTH,Pattern.SAME_SLOT, BuffState.PERM, BuffTriggerType.ON_PLAY, true, Math.Abs(damage) * -1));
        }
        public FireSpell() { }
        public override void ActivateSpell(ICard targer)
        {
            base.ActivateSpell(targer);
        }

        public override Card Clone(Card clone)
        {

            clone = base.Clone(clone);

            ((FireSpell)clone)._damage = _damage;
            return clone;
        }

        public override Card GetNewInstance()
        {
            return new FireSpell();
        }
    }
}
