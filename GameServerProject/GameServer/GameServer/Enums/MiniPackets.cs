using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Enums
{
    public enum MiniPackets
    {
       END_TURN = 0,
       GAME_START,
       ROUND_STATE,
       GAME_END,
    }
}
