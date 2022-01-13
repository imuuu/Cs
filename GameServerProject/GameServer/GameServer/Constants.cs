using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class Constants
    {
        public const int TICKS_PER_SECOND = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SECOND;
        //public const int TOTAL_PLAYERS_TO_START = 2;
        public const int ACTIONPOINT_COST_PLAYING_CARD_ON_TABLE = 1;
        public const int ACTIONPOINT_COST_ATTACKING_WITH_CARD = 1;
        public const bool DEBUG_ENABLED = false;
        public const int DECK_INCOME_AMOUNT = 50;
    }
}
