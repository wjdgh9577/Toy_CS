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
    void HandleSPing(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_Ping packet = (S_Ping)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSPing", packet.ServerTime.ToDateTime());

        gameSession.OnPing(packet.ServerTime.ToDateTime());
    }

    void HandleSConnected(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_Connected packet = (S_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSConnected", packet.ServerTime.ToDateTime());

        gameSession.OnPing(packet.ServerTime.ToDateTime());
    }

    void HandleSEnterRoom(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_EnterRoom packet = (S_EnterRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSEnterRoom", packet.EnterOk, packet.RoomId);
    }

    void HandleSLeaveRoom(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_LeaveRoom packet = (S_LeaveRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSLeaveRoom", packet.LeaveOk, packet.RoomId);
    }

    void HandleSTestChat(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_TestChat packet = (S_TestChat)message;
        LogHandler.Log(LogCode.CONSOLE, $"From: Session_{packet.Suid}, Chat: {packet.Chat}");
    }
}
