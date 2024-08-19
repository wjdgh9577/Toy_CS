using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Room;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    void HandleCPong(SessionBase session, IMessage message)
    {

    }

    void HandleCEnterRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_EnterRoom packet = (C_EnterRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCEnterRoom", packet.RoomId);

        RoomManager.Instance.EnterRoom<GameRoom>(clientSession, packet.RoomId);
    }

    void HandleCLeaveRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_LeaveRoom packet = (C_LeaveRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCLeaveRoom", packet.RoomId);

        RoomManager.Instance.LeaveRoom<GameRoom>(clientSession, packet.RoomId);
    }

    void HandleCTestChat(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_TestChat packet = (C_TestChat)message;
        LogHandler.Log(LogCode.CONSOLE, $"Chat: {packet.Chat}");

        S_TestChat broadcastPacket = new S_TestChat();
        broadcastPacket.Chat = packet.Chat;
        broadcastPacket.Suid = session.SUID;
        RoomManager.Instance.Broadcast<GameRoom>(clientSession, broadcastPacket);
    }
}
