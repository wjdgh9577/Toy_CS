using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;

public partial class PacketHandler
{
    public static void SendCPing()
    {
        C_Ping packet = new C_Ping();
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCConnected(string token)
    {
        C_Connected packet = new C_Connected();
        packet.Token = token;
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCEnterWaitingRoom(int uniqueId, string password)
    {
        C_EnterWaitingRoom packet = new C_EnterWaitingRoom();
        packet.UniqueId = uniqueId;
        packet.Password = password;
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCLeaveWaitingRoom(int uniqueId)
    {
        C_LeaveWaitingRoom packet = new C_LeaveWaitingRoom();
        packet.UniqueId = uniqueId;
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCRefreshWaitingRoom()
    {
        C_RefreshWaitingRoom packet = new C_RefreshWaitingRoom();
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCQuickEnterWaitingRoom()
    {
        C_QuickEnterWaitingRoom packet = new C_QuickEnterWaitingRoom();
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCCreateWaitingRoom(int type, int maxPersonnel, string title, string password)
    {
        C_CreateWaitingRoom packet = new C_CreateWaitingRoom();
        packet.Type = type;
        packet.MaxPersonnel = maxPersonnel;
        packet.Title = title;
        packet.Password = password;
        Managers.Instance.NetworkManager.Send(packet);
    }

    public static void SendCChat(string chat)
    {
        C_Chat packet = new C_Chat();
        packet.Chat = chat;
        Managers.Instance.NetworkManager.Send(packet);
    }
}
