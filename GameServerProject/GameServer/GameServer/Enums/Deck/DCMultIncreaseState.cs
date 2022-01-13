using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Enums
{
    public enum DCMultIncreaseState
    {
        ALONE = 0, //increase only when bought by self
        TOGETHER = 1, //increase when any of type has been bought
    }
}
