using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Events
{
    public static readonly EventPoint onGameStart = new EventPoint();
    public static readonly EventPoint onDrawCard = new EventPoint();
    public static readonly EventPoint notEnoughMana = new EventPoint();
    public static readonly EventPoint notEnoughMoney = new EventPoint();
    public static readonly EventPoint onHandFull = new EventPoint();
    public static readonly EventPoint onRefreshPlayerData = new EventPoint();

    public static readonly EventPoint<string> onGameStart1 = new EventPoint<string>();

    public static readonly EventPoint<AreaTriggerDrop> onAreaTriggerDrop = new EventPoint<AreaTriggerDrop>();
    public static readonly EventPoint<AreaTriggerEnter> onAreaTriggerEnter = new EventPoint<AreaTriggerEnter>();



}