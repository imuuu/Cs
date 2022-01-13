using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Interfaces
{
    public interface IScene
    {
        public void Join(Client client1, Client client2);
        public void EndGameScene(int gameSceneID);

        public bool IsHere(Client client);

        public void Disconnect(int clientID);
    }
}
