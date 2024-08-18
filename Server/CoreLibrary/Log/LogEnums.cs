using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Log
{
    public enum LogCode
    {
        // Debug
        CONSOLE,

        // Socket
        SOCKET_ERROR,

        // Session
        SESSION_INVALID_TYPE,
        SESSION_INVALID_UID,
        SESSION_NOT_EXIST,

        // Room
        ROOM_NOT_EXIST,
        ROOM_SESSION_INVALID_UID,
        ROOM_SESSION_NOT_EXIST,

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
}