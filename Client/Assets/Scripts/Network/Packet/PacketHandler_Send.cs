using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;

public partial class PacketHandler
{
    public static C_Ping C_Ping()
    {
        C_Ping packet = new C_Ping();

        return packet;
    }

    public static C_Connected C_Connected()
    {
        C_Connected packet = new C_Connected();

        return packet;
    }

    public static C_EnterWaitingRoom C_EnterWaitingRoom(int uniqueId, string password)
    {
        C_EnterWaitingRoom packet = new C_EnterWaitingRoom()
        {
            UniqueId = uniqueId,
            Password = password
        };

        return packet;
    }

    public static C_LeaveWaitingRoom C_LeaveWaitingRoom(int uniqueId)
    {
        C_LeaveWaitingRoom packet = new C_LeaveWaitingRoom()
        {
            UniqueId = uniqueId
        };

        return packet;
    }

    public static C_ReadyWaitingRoom C_ReadyWaitingRoom(bool ready)
    {
        C_ReadyWaitingRoom packet = new C_ReadyWaitingRoom()
        {
            Ready = ready
        };

        return packet;
    }

    public static C_RefreshLobby C_RefreshLobby()
    {
        C_RefreshLobby packet = new C_RefreshLobby();

        return packet;
    }

    public static C_QuickEnterWaitingRoom C_QuickEnterWaitingRoom()
    {
        C_QuickEnterWaitingRoom packet = new C_QuickEnterWaitingRoom();

        return packet;
    }

    public static C_CreateWaitingRoom C_CreateWaitingRoom(int type, int maxPersonnel, string title, string password)
    {
        C_CreateWaitingRoom packet = new C_CreateWaitingRoom()
        {
            Type = type,
            MaxPersonnel = maxPersonnel,
            Title = title,
            Password = password
        };

        return packet;
    }

    public static C_Chat C_Chat(string chat)
    {
        C_Chat packet = new C_Chat()
        {
            Chat = chat
        };

        return packet;
    }

    public static C_EnterGameRoom C_EnterGameRoom(GameRoomPlayerInfo info)
    {
        C_EnterGameRoom packet = new C_EnterGameRoom()
        {
            Info = info.GetProto()
        };

        return packet;
    }

    public static C_SyncPlayer C_SyncPlayer(GameRoomPlayerInfo info)
    {
        C_SyncPlayer packet = new C_SyncPlayer()
        {
            Info = info.GetProto()
        };

        return packet;
    }
}
