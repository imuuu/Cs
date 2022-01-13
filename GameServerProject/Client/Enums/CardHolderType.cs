using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum CardHolderType
{
    // I think if these can be manipulaited by hacks it might give hacker upper hand or crash the both games => in future closer look
    FRONTLINE = 0,
    BACKLINE,
    MIRROR_FRONTLINE,
    MIRROR_BACKLINE,
    HEAD,
    MIRROR_HEAD,
}