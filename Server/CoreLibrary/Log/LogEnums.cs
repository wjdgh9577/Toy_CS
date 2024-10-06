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

        // Packet
        PACKET_INVALID_TOKEN,

        // Session
        SESSION_INVALID_TYPE,
        SESSION_INVALID_UID,
        SESSION_NOT_EXIST,

        // Room
        ROOM_NOT_EXIST,
        ROOM_MAX_PERSONNEL,
        ROOM_SESSION_INVALID_UID,
        ROOM_SESSION_NOT_EXIST,

        // Game
        GAME_INVALID_MAPID,

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