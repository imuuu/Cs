using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace GameServer
{
    public class Client
    {
        public static int _dataBufferSize = 4096;

        public int _id;
        public Player _player;
        public TCP _tcp;
        public UDP _udp;
        
        public Client(int clientId)
        {
            _id = clientId;
            _tcp = new TCP(_id);
            _udp = new UDP(_id);
        }

        public bool IsClientConnected()
        {
            return _tcp._socket == null ? false : true;
        }

        public class TCP
        {
            public TcpClient _socket;

            private readonly int _id;
            private NetworkStream _stream;
            private Packet _receivedData;
            private byte[] _receiveBuffer;

            public TCP(int id)
            {
                _id = id;
            }

            public void Connect(TcpClient socket)
            {
                _socket = socket;
                _socket.ReceiveBufferSize = _dataBufferSize;
                _socket.SendBufferSize = _dataBufferSize;

                _stream = _socket.GetStream();
                _receivedData = new Packet();
                _receiveBuffer = new byte[_dataBufferSize];
                _stream.BeginRead(_receiveBuffer, 0, _dataBufferSize, ReceiveCallBack, null);

                ServerSend.Welcome(_id, "Welcome to the Card Game Server");

                Events.onConnect.Invoke(_id);
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (_socket != null)
                    {
                        _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sendiing data to player {_id} via TCP: {e}");
                }
            }

            private void ReceiveCallBack(IAsyncResult result)
            {
                try
                {
                    int byteLength = _stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        Server._clients[_id].Disconnect();
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
                    Server._clients[_id].Disconnect();
                }
            }

            private bool HandleData(byte[] data)
            {
                int packetLenght = 0;
                _receivedData.SetBytes(data);
                if (_receivedData.UnreadLength() >= 4)
                {
                    packetLenght = _receivedData.ReadInt();
                    if (packetLenght <= 0)
                    {
                        return true;
                    }
                }

                while (packetLenght > 0 && packetLenght <= _receivedData.UnreadLength())
                {
                    byte[] packetBytes = _receivedData.ReadBytes(packetLenght);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packetBytes))
                        {
                            int packetId = packet.ReadInt();
                            
                            Server._packetHandlers[packetId](_id,packet);
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

            public void Disconnect()
            {
                Events.onDisconnect.Invoke(_id);
                _socket.Close();
                _stream = null;
                _receivedData = null;
                _receiveBuffer = null;
                _socket = null;
            }
        }

        public class UDP
        {
            public IPEndPoint _endPoint;
            private int _id;

            public UDP(int id)
            {
                _id = id;
            }

            public void Connect(IPEndPoint endPoint)
            {
                _endPoint = endPoint;
            }

            public void SendData(Packet packet)
            {
                Server.SendUDPData(_endPoint, packet);
            }

            public void HandleData(Packet packetData)
            {
                int packetLength = packetData.ReadInt();
                byte[] packetBytes = packetData.ReadBytes(packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using(Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        Server._packetHandlers[packetId](_id, packet);
                    }
                });
            }

            public void Disconnect()
            {
                _endPoint = null;
            }
        }

        void SendIntoGame(string playerName)
        {
            //_player = new Player(_id, playerName);

            foreach (Client client in Server._clients.Values)
            {
                if(client._player != null)
                {
                    if(client._id != _id)
                    {
                        ServerSend.SpawnPlayer(_id, client._player);
                    }
                }
            }

            foreach (Client client in Server._clients.Values)
            {
                if (client._player != null)
                {
                    ServerSend.SpawnPlayer(client._id, _player);
                }
            }
        }

        public void SendCardToHand(Card card)
        {
            ServerSend.AddCardToHand(_id, card);
        }

        private void Disconnect()
        {
            Console.WriteLine($"{_tcp._socket.Client.RemoteEndPoint} has disconnected.");
            _tcp.Disconnect();
            _udp.Disconnect();
        }
    }
}
