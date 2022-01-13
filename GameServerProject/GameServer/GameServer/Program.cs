using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using GameServer.Managers;

namespace GameServer
{
    class Program
    {
        private static bool _isRunning = false;
        //List<Server> _servers = new List<Server>();
        static void Main(string[] args)
        {
            Console.Title = "Card Game Server";
            _isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            Server.Start(20, 26960); 
            
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SECOND} ticks per second.");

            DateTime nextLoop = DateTime.Now;

            while(_isRunning)
            {
                while(nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if(nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }

        public static T Clone<T>(T obj)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter= new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        
    }
}
