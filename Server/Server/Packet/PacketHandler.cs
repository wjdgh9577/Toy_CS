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
    void HandleLogic(Action<SessionBase, IMessage> handler, SessionBase session, IMessage message)
    {
        handler.Invoke(session, message);
    }

    #region Handler

    void HandleCPing(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;

        clientSession.RecvPing();
    }

    void HandleCConnected(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_Connected packet = (C_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCConnected", packet.Name);

        // TODO: PlayerInfo 저장
    }

    void HandleCEnterWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_EnterWaitingRoom packet = (C_EnterWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCEnterRoom", packet.ToString());

        S_EnterWaitingRoom resPacket = new S_EnterWaitingRoom();
        var result = RoomManager.Instance.EnterRoom<WaitingRoom>(clientSession, packet.UniqueId);
        resPacket.RoomInfo = result?.Info.GetProto();
        resPacket.EnterOk = result != null;
        clientSession.Send(resPacket);
    }

    void HandleCLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_LeaveWaitingRoom packet = (C_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCLeaveRoom", packet.ToString());

        S_LeaveWaitingRoom resPacket = new S_LeaveWaitingRoom();
        resPacket.LeaveOk = RoomManager.Instance.LeaveRoom<WaitingRoom>(clientSession, packet.UniqueId);
        clientSession.Send(resPacket);
    }

    void HandleCRefreshWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_RefreshWaitingRoom packet = (C_RefreshWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCRefreshRoom");

        S_RefreshWaitingRoom resPacket = new S_RefreshWaitingRoom();
        List<WaitingRoom> roomList = RoomManager.Instance.GetRooms<WaitingRoom>();
        foreach (WaitingRoom room in roomList)
            resPacket.RoomInfos.Add(room.Info.GetProto());
        clientSession.Send(resPacket);
    }

    void HandleCQuickEnterWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_QuickEnterWaitingRoom packet = (C_QuickEnterWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCQuickEnterRoom");

        var result = RoomManager.Instance.QuickWaitingRoom(clientSession);
        S_EnterWaitingRoom resPacket = new S_EnterWaitingRoom();
        resPacket.RoomInfo = result?.Info.GetProto();
        resPacket.EnterOk = result != null;
        clientSession.Send(resPacket);
    }

    void HandleCCreateWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_CreateWaitingRoom packet = (C_CreateWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCCreateRoom", packet.ToString());

        var result = RoomManager.Instance.CreateWaitingRoom(clientSession, packet.Type, packet.MaxPersonnel, packet.Title, packet.Password);
        S_EnterWaitingRoom resPacket = new S_EnterWaitingRoom();
        resPacket.RoomInfo = result?.Info.GetProto();
        resPacket.EnterOk = result != null;
        clientSession.Send(resPacket);
    }

    void HandleCChat(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_Chat packet = (C_Chat)message;
        LogHandler.Log(LogCode.CONSOLE, $"Chat: {packet.Chat}");

        S_Chat broadcastPacket = new S_Chat();
        broadcastPacket.Chat = packet.Chat;
        broadcastPacket.Suid = session.SUID;
        RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, broadcastPacket);
    }

    #endregion
}
