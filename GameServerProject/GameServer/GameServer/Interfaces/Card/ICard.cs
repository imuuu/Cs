using GameServer.Enums;
using System;

namespace GameServer
{
    public interface ICard
    {
        int _battlefieldSlotID { get; set; }
        string _description { get; set; }
        int _id { get; set; }
        string _name { get; set; }
        int _ownerClientID { get; set; }
        CardState _state { get; set; }
        Guid _uuid { get; set; }

        int _manaCost { get; set; }

        CardClassType _classType { get; set; }

        public Card Clone(Card clone);

        public Card GetNewInstance();
        public bool Equals(Card card);
        public int GetOwner();
        public void SetNewUUID();
        public void SetOwner(int clientID);
        public void PrintData() { }

        public Card BelongsTo(HeroEnum heroEnum);
        public Card BelongsTo(HeroEnum[] heroEnum);

        public bool HasHerosCard(Heroo hero);

    }
}