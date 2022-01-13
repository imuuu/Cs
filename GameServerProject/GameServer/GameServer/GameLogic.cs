using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Managers;
namespace GameServer
{
    class GameLogic
    {
      
        //Calling this 30 times in second!
        public static void Update()
        {
            ThreadManager.UpdateMain();
            RoundChecker();



        }

        private static void RoundChecker()
        {
            foreach(KeyValuePair<int, RoundManager> entry in Server._gameScene.GetAllRoundManagers())
            {
                int id = entry.Key;
                RoundManager roundManager = entry.Value;

                if (roundManager.isStarted()) //Server._roundManager != null
                {
                    
                    if (Server._gameScene.GetCooldowns(id).isCooldownReady("round"))
                    {
                        int playerID = roundManager.SwitchNextPlayer();
                        
                        //ServerSend.SendMiniPacket(playerID, Enums.MiniPackets.END_TURN, 1, roundManager.GetRoundTimeInSeconds());
                        //ServerSend.SendMiniPacket(-playerID, Enums.MiniPackets.END_TURN, 0, 0);
                        Console.WriteLine("Round timer has been come to end!");

                        roundManager.RoundStarts(playerID);
                       
                    }
                }
            }
            
        }
    }
}
