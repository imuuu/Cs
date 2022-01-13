using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Interfaces;
using GameServer.Managers;
namespace GameServer
{
    public class GameInfo
    {
        public readonly Client _owner;
        public readonly Client _opponent;
        public readonly int _id;
        public GameInfo(Client owner, Client opponent, int id)
        {
            _owner = owner;
            _opponent = opponent;
            _id = id;
        }
    }
    public class GameScene : IScene
    {
        Dictionary<int, GameInfo> _games = new Dictionary<int, GameInfo>();
        
        Dictionary<int, RoundManager> _roundManagers = new Dictionary<int, RoundManager>();
        Dictionary<int, Cooldowns> _cooldowns = new Dictionary<int, Cooldowns>();
        Dictionary<int, BattlefieldManager> _battlefieldManager = new Dictionary<int, BattlefieldManager>();
        //public static Cooldowns _cooldowns = _cooldowns = new Cooldowns();
        int _managerCount = 0;


        public GameScene()
        {
            Events.onDisconnect.AddListener(Disconnect);
        }

        public void Disconnect(int clientID)
        {
            //if (_games.ContainsKey(clientID)) _games.Remove(clientID);
            if (!_games.ContainsKey(clientID)) return;
            
            GameInfo gi = GetGameInfo(clientID);
            EndGameScene(gi._id);
            //GameHasEnded(gi._id, gi._opponent, gi._owner);
        }

        public bool IsHere(Client client)
        {
            return _games.ContainsKey(client._id);
        }

        public GameInfo GetGameInfo(int clientID)
        {
            //Console.WriteLine("id " + id);
            return _games[clientID];
        }

      
        public RoundManager GetRoundManager(Client client)
        {
            return _roundManagers[_games[client._id]._id];
        }
        public RoundManager GetRoundManager(int id)
        {
            return _roundManagers[id];
        }

        public BattlefieldManager GetBattlefieldManager(int id)
        {
            return _battlefieldManager[id];
        }

        public Cooldowns GetCooldowns(int id)
        {
            return _cooldowns[id];
        }

        public Dictionary<int, RoundManager> GetAllRoundManagers()
        {
            return _roundManagers;
        }

        public void Join(Client client1, Client client2)
        {
            GameInfo si1 = new GameInfo(client1, client2, _managerCount);
            GameInfo si2 = new GameInfo(client2, client1, _managerCount);
           

            _games[client1._id] = si1;
            _games[client2._id] = si2;
            //Events.onJoinGameScene.Invoke(client1._id);
            //Events.onJoinGameScene.Invoke(client2._id);

            Console.WriteLine($"Client: {client1._id} and {client2._id} has started battle waiting for game scene open");
            
            _roundManagers[_managerCount] = new RoundManager(_managerCount, new int[] { client1._id, client2._id });
            _battlefieldManager[_managerCount] = new BattlefieldManager(_managerCount, new int[] {client1._id,client2._id}, Server._cardColManager);
            _cooldowns[_managerCount] = new Cooldowns();
            
            
            ServerSend.SendToScene(client1._id, Enums.Scene_enum.GAME_SCENE);
            ServerSend.SendToScene(client2._id, Enums.Scene_enum.GAME_SCENE);

            _managerCount++;
            
        }

        public void GameHasEnded(int gameSceneID, Client winner, Client loser)
        {
            Console.WriteLine($"Game has ended! Client {winner._id} has won the game agains {loser._id}");
            EndGameScene(gameSceneID);
        }

        public void EndGameScene(int gameSceneID)
        {
            GameInfo gameInfo = null;
            foreach(var gi in _games.Values)
            {
                if(gi._id == gameSceneID)
                {
                    gameInfo = gi;
                    break;
                }
            }

            _roundManagers.Remove(gameSceneID);
            _battlefieldManager.Remove(gameSceneID);
            _cooldowns.Remove(gameSceneID);

            foreach(int clientId in new int[] { gameInfo._owner._id, gameInfo._opponent._id})
            {
                _games.Remove(clientId);

                if (!Server._clients[clientId].IsClientConnected()) continue;

                Server._lobby.Join(Server._clients[clientId]);
                ServerSend.SendToScene(clientId, Enums.Scene_enum.MAIN_MENU);
                Events.onLeaveGameScene.Invoke(clientId);
            }

            //_games.Remove(gameInfo._owner._id);
            //_games.Remove(gameInfo._opponent._id);

            //Server._lobby.Join(Server._clients[gameInfo._owner._id]);
            //Server._lobby.Join(Server._clients[gameInfo._opponent._id]);


            //ServerSend.SendToScene(gameInfo._owner._id, Enums.Scene_enum.MAIN_MENU);
            //ServerSend.SendToScene(gameInfo._opponent._id, Enums.Scene_enum.MAIN_MENU);

            //Events.onLeaveGameScene.Invoke(gameInfo._owner._id);
            //Events.onLeaveGameScene.Invoke(gameInfo._opponent._id);
        }
    }
}
