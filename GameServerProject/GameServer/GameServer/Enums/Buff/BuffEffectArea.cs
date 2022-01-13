using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Enums
{
    public enum BuffEffectArea
    {
        //ORDER MATTERS! => BuffCardManager
       NONE = 0,
       BUFF_FRONT_CARD,
       BUFF_BEHIND_CARD,
       BUFF_SIDE_CARDS,
       BUFF_AROUND_CARDS,
       BUFF_CROSS_CARDS,
       BUFF_ALL_CARDS,
       BUFF_FRONTLINE_CARDS,
       BUFF_BACKLINE_CARDS
       //BUFF_FACTION_CARDS, // laittaa et mitä facsönii
       //BUFF_SAME_TYPE_CARDS
    }
}
