using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameServer.Enums;

namespace GameServer.Managers
{
    public class RoundManager
    {
        private int _id;
        private int[] _clientIDs;
        private int _rounds = 0;
        private int _playerTurnID = 1;
        private RoundState _state = RoundState.PURCHACE;
        private int _roundTimeS = 60;
        private bool _started = false;
        private bool[] _allReady;
        public RoundManager(int id, int[] clientIDs)
        {
            _id = id;
            _clientIDs = clientIDs;
            _allReady = new bool[_clientIDs.Length];
        }

        public int GetRoundNumber()
        {
            return _rounds;
        }

        public void AddRounds(int amount)
        {
            _rounds += amount;
        }

        public void SetRoundNumber(int round)
        {
            _rounds = round;
        }

        public void SetPlayerTurn(int playerID)
        {
            _playerTurnID = playerID;
        }

        public int GetPlayerWhosTurn()
        {
            return _playerTurnID;
        }

        public bool isPlayerTurn(int playerID)
        {
            if (_playerTurnID == playerID)
            {
                return true;
            }

            if (Constants.DEBUG_ENABLED)
            {
                return true;
            }


            Console.WriteLine("Its not ur turn!");
            return false;
        }

        public RoundState GetState()
        {
            return _state;
        }
        public RoundState SwitchState()
        {
            if (_state == RoundState.PURCHACE)
            {
                _state = RoundState.FIGHT;
            }
            else
            {
                _state = RoundState.PURCHACE;
            }

            return _state;    
            
        }

        public void ButtonPressEndTurn()
        {
            Console.WriteLine("Button press end turn: "+GetPlayerWhosTurn());
            if(_state == RoundState.PURCHACE)
            {
                _state = RoundState.FIGHT;
                ServerSend.SendMiniPacket(_playerTurnID, Enums.MiniPackets.ROUND_STATE, 1, 0);
                return;
            }
            else
            {
                _state = RoundState.PURCHACE;
                ServerSend.SendMiniPacket(_playerTurnID, Enums.MiniPackets.ROUND_STATE, 0, 0);
            }

            int lastPlayerID = _playerTurnID;
            int playerID = SwitchNextPlayer();
            if(lastPlayerID != playerID)
            {
                _state = RoundState.PURCHACE;
            }
            RoundStarts(playerID);
           
        }

        public void RoundStarts(int clientId)
        {
            Server._gameScene.GetCooldowns(_id).SetCooldownInSeconds("round", GetRoundTimeInSeconds());
            Events.onRoundStart.Invoke(clientId);

            //Server._cooldowns.SetCooldownInSeconds("round", Server._roundManager.GetRoundTimeInSeconds());

            ServerSend.SendMiniPacket(clientId, Enums.MiniPackets.END_TURN, 1, GetRoundTimeInSeconds());
            
            //ServerSend.SendMiniPacket(-clientId, Enums.MiniPackets.END_TURN, 0, 0);
            ServerSend.SendMiniPacket(Server.GetOpponentForId(clientId), Enums.MiniPackets.END_TURN, 0, 0);

        }
        public void SetState(RoundState state)
        {
            _state = state;
        }

        public void SetRoundTimeInSeconds(int time)
        {
            _roundTimeS = time;
        }

        public int GetRoundTimeInSeconds()
        {
            return _roundTimeS;
        }

        public bool isStarted()
        {
            return _started;
        }

        public void SetStarted()
        {
            if(!_started)
            {
                SetPlayerTurn(0);
                foreach(int clientID in _clientIDs)
                {
                    ServerSend.SendMiniPacket(clientID, MiniPackets.GAME_START, 1, 0);
                    ServerSend.SendMiniPacket(clientID, Enums.MiniPackets.ROUND_STATE, 0, 0);              
                }
                Events.onGameStart.Invoke();
            }
            _started = true;   
        }

        public int SwitchNextPlayer()
        {
            _playerTurnID = Server.GetOpponentForId(_playerTurnID);
            return _playerTurnID;
            //int i = 1;
            //for(; i <= Server._maxPlayers; ++i)
            //{
            //    if(Server._clients[i]._tcp._socket != null && i == _playerTurnID)
            //    {                   
            //        if(Server._clients.ContainsKey(i + 1) && Server._clients[i + 1]._tcp._socket != null)
            //        {
            //            _playerTurnID = i + 1;
            //            //Console.WriteLine("Next round id is: " + _playerTurnID);
            //            return _playerTurnID;
            //        }
                   
            //    }
            //}

            //if ((Server._clients.ContainsKey(i+1) && Server._clients[i + 1]._tcp._socket == null) || i > Server._maxPlayers)
            //{
            //    for (int l = 1; l <= Server._maxPlayers; ++l)
            //    {
            //        if (Server._clients[l]._tcp._socket != null)
            //        {
            //            _playerTurnID = l;
            //            //Console.WriteLine("new Next round id is: " + _playerTurnID);
            //            return _playerTurnID;
            //        }
            //    }
            //    _playerTurnID = 0;
            //    //Console.WriteLine("Couldnt find client for next round!");
            //    return _playerTurnID;
            //}
            
            
            //return 0;
        }
        bool Allready()
        {     
            foreach(bool rd in _allReady)
            {
                if (rd == false) return false;
            }
            return true;
        }

        public void GameHasEnded(int winnerClientID)
        {
            
            GameInfo gameInfo = Server._gameScene.GetGameInfo(winnerClientID);
            
            ServerSend.SendMiniPacket(gameInfo._owner._id, MiniPackets.GAME_END, 1, gameInfo._opponent._player._username);
            ServerSend.SendMiniPacket(gameInfo._opponent._id, MiniPackets.GAME_END, 0, gameInfo._owner._player._username);
            
            Task.Delay(3000).ContinueWith(m => Server._gameScene.GameHasEnded(gameInfo._id, gameInfo._owner, gameInfo._owner));
            Console.WriteLine("Sending to lobby...");
        }
        public void StartTheGame(int fromClient)
        {
            Console.WriteLine($"Preparing match for client {fromClient}");
            Client client = Server._clients[fromClient];

            //Server._clients[fromClient].SendIntoGame(client._player._username);
            ServerSend.SpawnPlayer(client._id, client._player);
            ServerSend.SpawnPlayer(client._id, Server._clients[Server.GetOpponentForId(client._id)]._player);
            
            for(int i = 0; i < _clientIDs.Length; ++i)
            {
                if(_clientIDs[i] == fromClient)
                {
                    _allReady[i] = true;
                    break;
                }
            }



            if(Allready())
            {
                Console.WriteLine("===> Start the match!");
                SetStarted();
                _playerTurnID = fromClient;
                for (int i = 0; i < _clientIDs.Length; ++i)
                {
                    Console.WriteLine($"Sending i:{i} clientid: {_clientIDs[i]}");
                    Server._heroManager.SendHeroesToClients(_clientIDs[i]);
                    Events.onJoinGameScene.Invoke(_clientIDs[i]);
                }
                    
            }
        }
    }
}
