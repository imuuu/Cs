using System;
namespace GameServer.Managers
{
    public class BattleSlot
    {
        public Guid _triggerUUID;
        public int _ownerID;
        public int _slot = -1;
        public BattleSlot(Guid triggerUUID, int clientID, int slot)
        {
            _triggerUUID = triggerUUID;
            _ownerID = clientID;
            _slot = slot;
        }
        
        public Card GetCard()
        {
            if (_slot < 0)
                return null;

            return Server._clients[_ownerID]._player.GetBattlefield().GetCard(_slot);
        }
    }
}
