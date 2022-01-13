using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
namespace GameServer
{
    public static class Events
    {
        public static readonly EventPoint onGameStart = new EventPoint();
        public static readonly EventPoint<int> onRoundStart = new EventPoint<int>(); //client id

        public static readonly EventPoint<int> onConnect = new EventPoint<int>();
        public static readonly EventPoint<int> onDisconnect = new EventPoint<int>();

        public static readonly EventPoint<int> onJoinLobby = new EventPoint<int>();
        public static readonly EventPoint<int> onJoinGameScene = new EventPoint<int>();

        public static readonly EventPoint<int> onLeaveLobby = new EventPoint<int>();
        public static readonly EventPoint<int> onLeaveGameScene = new EventPoint<int>();

        public static readonly EventPoint<Card> onCardDestroy = new EventPoint<Card>();
        public static readonly EventPoint<Card> onCardPlay = new EventPoint<Card>();
        public static readonly EventPoint<Card> onCardTakeDamage = new EventPoint<Card>(); 
        public static readonly EventPoint<Card> onCardHeal = new EventPoint<Card>();
        public static readonly EventPoint<ActionTriggerPlate> onActionTrigger = new EventPoint<ActionTriggerPlate>();
    }
}
