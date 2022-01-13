using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GameServer.Managers;

namespace GameServer
{
    class Server
    {
        public static int _maxPlayers { get; private set; }
        public static int _port { get; private set; }

        public static Dictionary<int, Client> _clients = new Dictionary<int, Client>();

        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> _packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;


        public static Lobby _lobby;
        public static GameScene _gameScene;

        public static CardCollectionManager _cardColManager;
        //public static RoundManager _roundManager;
        //public static BattlefieldManager _battlefieldManager;
        public static DeckManager _deckManager;
        public static HeroManager _heroManager;
        public static AreaTriggerManager _areaTriggerManager;
        
        //public static Cooldowns _cooldowns = _cooldowns = new Cooldowns();


        public static void Start(int maxPlayers, int port)
        {
            _maxPlayers = maxPlayers;
            _port = port;
            
            _lobby = new Lobby();
            _gameScene = new GameScene();

            _cardColManager = new CardCollectionManager();
            //_roundManager = new RoundManager();
            //_battlefieldManager = new BattlefieldManager(_cardColManager);
            _heroManager = new HeroManager();
            _deckManager = new DeckManager();
            _areaTriggerManager = new AreaTriggerManager();
           

            Console.WriteLine("Starting card game server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, _port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TPCConnectCallBack), null);

            udpListener = new UdpClient(_port);
            udpListener.BeginReceive(UDPReceiveCallBack, null);

            Console.WriteLine("Server started on " + _port);
   
        }

        public static RoundManager GetRoundManager(int clientID)
        {
            return _gameScene.GetRoundManager(_clients[clientID]);
        }

        public static BattlefieldManager GetBattlefieldManager(int clientID)
        {
            return _gameScene.GetBattlefieldManager(_gameScene.GetGameInfo(clientID)._id);
        }
        public static int GetOpponentForId(int clientID)
        {
            //if(clientID == 1)
            //{
            //    return 2;
            //}
            //return 1;
            //int opponentID;
            //Console.WriteLine($"Asking opponent for {clientID} and it is: ");
            return _gameScene.GetGameInfo(clientID)._opponent._id;
        }
        private static void TPCConnectCallBack(IAsyncResult result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TPCConnectCallBack), null);
            Console.WriteLine("Incoming connection from " + client.Client.RemoteEndPoint+"...");

            for(int i = 1; i <= _maxPlayers; i++)
            {
                if(_clients[i]._tcp._socket == null)
                {
                    _clients[i]._tcp.Connect(client);
                    return;
                }
            }

            Console.WriteLine(client.Client.RemoteEndPoint + " Failed to connect: Server full!");
        }

        private static void UDPReceiveCallBack(IAsyncResult result)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpListener.EndReceive(result, ref clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallBack, null);

                if(data.Length < 4)
                {
                    return;
                }

                using(Packet packet = new Packet(data))
                {
                    int clientId = packet.ReadInt();

                    if(clientId == 0)
                    {
                        return;
                    }

                    if(_clients[clientId]._udp._endPoint == null)
                    {
                        _clients[clientId]._udp.Connect(clientEndPoint);
                        return;
                    }

                    if(_clients[clientId]._udp._endPoint.ToString() == clientEndPoint.ToString())
                    {
                        _clients[clientId]._udp.HandleData(packet);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error receiving UDP data: {e}");
            }
        }

        public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if(clientEndPoint != null)
                {
                    udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error sending data to {clientEndPoint} via UDP: {e}");
            }
        }

        private static void InitializeServerData()
        {
            for(int i = 1; i <= _maxPlayers; i++)
            {
                _clients.Add(i, new Client(i));
            }

            _packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived }, 
                {(int) ClientPackets.playerClickedDeck, ServerHandle.DeckClick },
                {(int) ClientPackets.playerDecks, ServerHandle.PlayerDecks},
                {(int) ClientPackets.playerChosenCard, ServerHandle.ChosenCard},
                {(int) ClientPackets.miniPackets, ServerHandle.ReceivedMiniPacket},
                {(int) ClientPackets.battleBetweenCards, ServerHandle.ReceivedBattleBetweenCards},
                {(int) ClientPackets.hitHero, ServerHandle.PlayerHitHead},
                {(int) ClientPackets.receiveChosenIndicatorCard, ServerHandle.ReceivedChosenIndicatorCard},
                {(int) ClientPackets.OnEnterCardHolder, ServerHandle.OnEnterCardHolder},
                {(int) ClientPackets.OnAreaTableTriggerDrop, ServerHandle.OnAreaTableTriggerDrop},
                {(int) ClientPackets.OnTriggerClassAbility, ServerHandle.OnClassAbilityTrigger},
                {(int) ClientPackets.SendChosenCollectionCards, ServerHandle.ReceivingChosenCollectionCards},
                {(int) ClientPackets.AskChosenHeroCards, ServerHandle.ReceivingAskHeroCards},
                {(int) ClientPackets.JoinAndLeaveQueue, ServerHandle.JoinAndLeaveQueue},
                {(int) ClientPackets.SceneLoaded, ServerHandle.SceneLoaded},
                {(int) ClientPackets.SendUsername, ServerHandle.RecevingUsername},


            };

            Console.WriteLine("Initialized packets.");
        }

       

        public static CardCollectionManager GetCardCollectionManager()
        {
            return _cardColManager;
        }
    }
}
