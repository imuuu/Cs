using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Interfaces;
using System.Linq;
namespace GameServer
{
    class Lobby 
    {
        Dictionary<int, Client> _clients = new Dictionary<int, Client>();
        Queue<int> _waitingForGame = new Queue<int>();
        //Dictionary<int, List<int>> _client_chosen_cards = new Dictionary<int, List<int>>();

        public Lobby()
        {
            Events.onDisconnect.AddListener(Disconnect);
            Events.onConnect.AddListener(Connect);
        }


        public void Connect(int clientID)
        {
            Console.WriteLine("Connect: " + clientID);
            Join(Server._clients[clientID]);
        }
        public void Disconnect(int clientID)
        {
            if (_clients.ContainsKey(clientID)) _clients.Remove(clientID);
            _waitingForGame = new Queue<int>(_waitingForGame.Where(x => x != clientID));
            //if (_client_chosen_cards.ContainsKey(clientID)) _client_chosen_cards.Remove(clientID);
        }

        public bool IsHere(Client client)
        {
            return _clients.ContainsKey(client._id);
        }

        public void JoinQueue(int clientID)
        {
            Console.WriteLine($"Client {clientID} joining queue");
            if (_waitingForGame.Count == 0)
            {
                _waitingForGame.Enqueue(clientID);
                Console.WriteLine($" ===> Client {clientID} has place in queue!");
                return;
            }
            Console.WriteLine($" ===> Client {clientID} has found opponent!");
            Client c1 = Server._clients[_waitingForGame.Dequeue()];
            Client c2 = Server._clients[clientID];
            Server._gameScene.Join(c1, c2);          
        }

        public void LeaveQueue(int clientID)
        {
            _waitingForGame = new Queue<int>(_waitingForGame.Where(x => x != clientID));
            Console.WriteLine($"client: {clientID} has left from queue");
        }

        public void Join(Client client)
        {
            Console.WriteLine($"Client: {client._id} has Join to Lobby");
           
            //if (!_client_chosen_cards.ContainsKey(client._id))
            //    _client_chosen_cards[client._id] = new List<int>();
            
            client._player = new Player(client._id, "");
            _clients[client._id] = client;
            Events.onJoinLobby.Invoke(client._id);

            ServerSend.SendHeroes(client._id);
            //ServerSend.SendCollectionCards(client._id, client._player.GetHero().get)
        }

        public void Leave(Client client)
        {
            _clients.Remove(client._id);
            Events.onLeaveLobby.Invoke(client._id);
        }

        //public void AddCard(Client client, int cardID)
        //{
        //    _client_chosen_cards[client._id].Add(cardID);
        //}

        //public void RemoveCard(Client client, int cardID)
        //{
        //    _client_chosen_cards[client._id].Remove(cardID);
        //}

        //public List<int> GetChosenCards(Client client)
        //{
        //    return _client_chosen_cards[client._id];
        //}

       
    }
}
