using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client _instance;
    public static int _dataBufferSize = 4096;

    public string _ip = "127.0.0.1";
    public int _port = 26950;
    public int _myId = 0;
    public TCP _tcp;
    public UDP _udp;

    private bool isConnected = false;

    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> _packetHandlers;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if(_instance != this)
        {
            Debug.Log("Instance already exists, destroy object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        _tcp = new TCP();
        _udp = new UDP();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public bool IsConnected()
    {
        return isConnected;
    }

    public void SetConnected(bool b)
    {
        isConnected = true;
    }

    public void ConnectedToServer()
    {
        //InitializeClientData();

        //isConnected = true;
        _tcp.Connect();
    }

    public class TCP
    {
        public TcpClient _socket;
        
        private NetworkStream _stream;
        private Packet _receivedData;
        private byte[] _receiveBuffer;

        public void Connect()
        {
            _socket = new TcpClient
            {
                ReceiveBufferSize = _dataBufferSize,
                SendBufferSize = _dataBufferSize
            };

            _receiveBuffer = new byte[_dataBufferSize];
            _socket.BeginConnect(_instance._ip, _instance._port, ConnectCallBack, _socket);
        }

        private void ConnectCallBack(IAsyncResult result)
        {
           

            _socket.EndConnect(result);
            if(!_socket.Connected)
            {
                return;
            }
            
            Client._instance.SetConnected(true);
            Client._instance.InitializeClientData();
            
            _stream = _socket.GetStream();

            _receivedData = new Packet();

            _stream.BeginRead(_receiveBuffer, 0, _dataBufferSize, ReceiveCallBack, null);

        }

        public void SendData(Packet packet)
        {
            try
            {
                if( _socket != null)
                {
                    _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }catch(Exception e)
            {
                Debug.Log($"Error sending data to server via TCP: {e}");
            }
        }

        private void ReceiveCallBack(IAsyncResult result)
        {
            try
            {
                int byteLength = _stream.EndRead(result);
                if (byteLength <= 0)
                {
                    _instance.Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(_receiveBuffer, data, byteLength);

                _receivedData.Reset(HandleData(data));

                _stream.BeginRead(_receiveBuffer, 0, _dataBufferSize, ReceiveCallBack, null);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error receiving TCP data: " + e);
                Disconnect();
            }
        }

        private bool HandleData(byte[] data)
        {
            int packetLenght = 0;
            _receivedData.SetBytes(data);
            if(_receivedData.UnreadLength() >= 4)
            {
                packetLenght = _receivedData.ReadInt();
                if(packetLenght <= 0)
                {
                    return true;
                }
            }
            
            while(packetLenght > 0 && packetLenght <= _receivedData.UnreadLength())
            {
                byte[] packetBytes = _receivedData.ReadBytes(packetLenght);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        _packetHandlers[packetId](packet);
                    }
                });

                packetLenght = 0;

                if (_receivedData.UnreadLength() >= 4)
                {
                    packetLenght = _receivedData.ReadInt();
                    if (packetLenght <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLenght <= 1)
            {
                return true;
            }
            return false;
               
        }

        private void Disconnect()
        {
            _instance.Disconnect();

            _stream = null;
            _receivedData = null;
            _receiveBuffer = null;
            _socket = null;
        }
    }

    public class UDP
    {
        public UdpClient _socket;
        public IPEndPoint _endPoint;

        public UDP()
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(_instance._ip), _instance._port);
        }

        public void Connect(int localPort)
        {
            _socket = new UdpClient(localPort);
            _socket.Connect(_endPoint);
            _socket.BeginReceive(ReceiveCallBack, null);

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }

        public void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(_instance._myId);
                if (_socket != null)
                {
                    _socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error sending data to server via UDP: {e}");
            }
        }

        private void ReceiveCallBack(IAsyncResult result)
        {
            try
            {
                byte[] data = _socket.EndReceive(result, ref _endPoint);
                _socket.BeginReceive(ReceiveCallBack, null);

                if (data.Length < 4)
                {
                    _instance.Disconnect();
                    return;
                }
                HandleData(data);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }
        private void HandleData(byte[] data)
        {
            using (Packet packet = new Packet(data))
            {
                int packetLenght = packet.ReadInt();
                data = packet.ReadBytes(packetLenght);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(data))
                {
                    int packetId = packet.ReadInt();
                    _packetHandlers[packetId](packet);
                }
            });
        }

        private void Disconnect()
        {
            _instance.Disconnect();
            _endPoint = null;
            _socket = null;
        }
        
    }
    public void InitializeClientData()
    {
        //received packets
        _packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.welcome, ClientHandle.Welcome },
                {(int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
                {(int)ServerPackets.cardToHand, ClientHandle.AddCardToHand },
                //{(int)ServerPackets.playerSendDroppedCardToHolder, ClientHandle.PutCardToHolder},
                {(int)ServerPackets.sendDroppedCardOtherPlayers, ClientHandle.PutCardToHolderMirror},
                {(int)ServerPackets.cardToHandOpponent, ClientHandle.AddCardToHandOpponent},
                {(int)ServerPackets.refreshPlayerData, ClientHandle.RefreshPlayerData},
                {(int)ServerPackets.initDeck, ClientHandle.InitDeck},
                {(int)ServerPackets.choseCardFromDeck, ClientHandle.ChoseCardFromDeck},
                {(int)ServerPackets.miniPackets, ClientHandle.ReceivedMiniPacket},
                {(int)ServerPackets.updateBattleCard, ClientHandle.UpdateBattleCard},
                {(int)ServerPackets.initHero, ClientHandle.InitHero},
                {(int)ServerPackets.updateBattlefieldCards, ClientHandle.UpdateBattlefieldCards},
                {(int)ServerPackets.receiveCardIndicators, ClientHandle.ReceiveCardIndicators},
                {(int)ServerPackets.responseAreaTrigger, ClientHandle.ResponseAreaTrigger},
                {(int)ServerPackets.initBattleSlots, ClientHandle.InitBattleSlots},
                {(int)ServerPackets.removeCardFromHand, ClientHandle.RemoveCardFromHand},
                {(int)ServerPackets.initClassAbility, ClientHandle.InitClassAbility},
                {(int)ServerPackets.initClassAbilityPassive, ClientHandle.InitClassAbilityPassive},
                {(int)ServerPackets.initHeroes, ClientHandle.InitHeroesToCollection},
                {(int)ServerPackets.SendCollectionCards, ClientHandle.ReceiveHeroColCards},
                {(int)ServerPackets.SendToScene, ClientHandle.ReceiveSceneLoad},
                
            };

        Debug.Log("Initialized packets.");
    }

    public void Disconnect()
    {
        if(isConnected)
        {
            isConnected = false;
            _tcp._socket.Close();
            _udp._socket.Close();
            GameManager._instance.RemovePlayerFromPlayers(_myId);
            Debug.Log("Disconnected from server.");
        }
    }
}


