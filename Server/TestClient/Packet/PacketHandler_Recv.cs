using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Session;

public partial class PacketHandler
{
    void HandleLogic(Action<SessionBase, IMessage> handler, SessionBase session, IMessage message)
    {
        handler.Invoke(session, message);
    }

    void HandleSPing(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Ping packet = (S_Ping)message;
        //LogHandler.Log(LogCode.CONSOLE, "HandleSPing", packet.ServerTime.ToDateTime());

        serverSession.OnPing(packet.ServerTime.ToDateTime());
    }

    void HandleSConnected(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Connected packet = (S_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSConnected", packet.ServerTime.ToDateTime());

        serverSession.OnPing(packet.ServerTime.ToDateTime());
    }

    void HandleSEnterWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_EnterWaitingRoom packet = (S_EnterWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSEnterRoom", packet.EnterOk, packet.RoomInfo?.BaseInfo.UniqueId);
    }

    void HandleSLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_LeaveWaitingRoom packet = (S_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSLeaveRoom", packet.LeaveOk);
    }

    void HandleSRefreshWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_RefreshWaitingRoom packet = (S_RefreshWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSRefreshRoom", packet.ToString());
    }

    void HandleSChat(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Chat packet = (S_Chat)message;
        LogHandler.Log(LogCode.CONSOLE, $"From: Session_{packet.Suid}, Chat: {packet.Chat}");
    }
}
