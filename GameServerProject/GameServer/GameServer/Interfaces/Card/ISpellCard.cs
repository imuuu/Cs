using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Cards;
namespace GameServer.Interfaces.Card
{
    public interface ISpellCard
    {
        public void ActivateSpell(ICard targer);
    }
}
