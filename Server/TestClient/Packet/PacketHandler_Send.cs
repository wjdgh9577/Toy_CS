using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Session;

public partial class PacketHandler
{
    public static C_Ping C_Ping()
    {
        C_Ping packet = new C_Ping();

        return packet;
    }

    public static C_Connected C_Connected(string token)
    {
        C_Connected packet = new C_Connected();
        packet.Token = token;

        return packet;
    }

    public static C_EnterWaitingRoom C_EnterWaitingRoom(int uniqueId, string password)
    {
        C_EnterWaitingRoom packet = new C_EnterWaitingRoom();
        packet.UniqueId = uniqueId;
        packet.Password = password;

        return packet;
    }

    public static C_LeaveWaitingRoom C_LeaveWaitingRoom(int uniqueId)
    {
        C_LeaveWaitingRoom packet = new C_LeaveWaitingRoom();
        packet.UniqueId = uniqueId;

        return packet;
    }

    public static C_RefreshWaitingRoom C_RefreshWaitingRoom()
    {
        C_RefreshWaitingRoom packet = new C_RefreshWaitingRoom();

        return packet;
    }

    public static C_QuickEnterWaitingRoom C_QuickEnterWaitingRoom()
    {
        C_QuickEnterWaitingRoom packet = new C_QuickEnterWaitingRoom();

        return packet;
    }

    public static C_CreateWaitingRoom C_CreateWaitingRoom(int type, int maxPersonnel, string title, string password)
    {
        C_CreateWaitingRoom packet = new C_CreateWaitingRoom();
        packet.Type = type;
        packet.MaxPersonnel = maxPersonnel;
        packet.Title = title;
        packet.Password = password;

        return packet;
    }

    public static C_Chat C_Chat(string chat)
    {
        C_Chat packet = new C_Chat();
        packet.Chat = chat;

        return packet;
    }
}
