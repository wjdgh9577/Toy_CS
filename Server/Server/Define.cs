using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server;

public static class Define
{
    public const int PING_INTERVAL = 5 * 1000;
    public const int PING_TIMEOUT = 30 * 1000;

    public const int ROOM_UPDATE_INTERVAL = 1000;
    public const int MAX_CCU = 500;
}
