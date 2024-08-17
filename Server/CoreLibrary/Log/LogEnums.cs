using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Log;

public enum LogCode
{
    // Debug
    CONSOLE,

    // Socket
    SOCKET_ERROR,

    // Room
    ROOM_NOT_EXIST,
    INVALID_SESSION_TYPE,

    // Exception
    EXCEPTION,
}

public enum LogType
{
    NONE,
    WARNING,
    ERROR,
}

public enum LogKey
{

}
