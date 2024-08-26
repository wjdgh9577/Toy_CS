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
        C_Ping packet = (C_Ping)message;
        //LogHandler.Log(LogCode.CONSOLE, "HandleCPing");

        clientSession.RecvPing();
    }

    void HandleCConnected(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_Connected packet = (C_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCConnected", packet.Token);

        clientSession.Token ??= packet.Token;
        // TODO: PlayerInfo 저장
    }

    void HandleCEnterWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_EnterWaitingRoom packet = (C_EnterWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCEnterRoom", packet.ToString());

        WaitingRoom? result = RoomManager.Instance.EnterWaitingRoom(clientSession, packet.UniqueId, packet.Password);
        WaitingRoomInfo? roomInfo = result?.Info.GetProto();
        bool enterOk = result != null;
        clientSession.Send(S_EnterWaitingRoom(roomInfo, enterOk));
    }

    void HandleCLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_LeaveWaitingRoom packet = (C_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCLeaveRoom", packet.ToString());

        bool leaveOk = RoomManager.Instance.LeaveRoom<WaitingRoom>(clientSession, packet.UniqueId);
        clientSession.Send(S_LeaveWaitingRoom(leaveOk));
    }

    void HandleCRefreshWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_RefreshWaitingRoom packet = (C_RefreshWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCRefreshRoom");

        List<WaitingRoom> roomList = RoomManager.Instance.GetRooms<WaitingRoom>();
        clientSession.Send(S_RefreshWaitingRoom(roomList));
    }

    void HandleCQuickEnterWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_QuickEnterWaitingRoom packet = (C_QuickEnterWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCQuickEnterRoom");

        WaitingRoom? result = RoomManager.Instance.QuickWaitingRoom(clientSession);
        WaitingRoomInfo? roomInfo = result?.Info.GetProto();
        bool enterOk = result != null;
        clientSession.Send(S_EnterWaitingRoom(roomInfo, enterOk));
    }

    void HandleCCreateWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_CreateWaitingRoom packet = (C_CreateWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCCreateRoom", packet.ToString());

        WaitingRoom? result = RoomManager.Instance.CreateWaitingRoom(clientSession, packet.Type, packet.MaxPersonnel, packet.Title, packet.Password);
        WaitingRoomInfo? roomInfo = result?.Info.GetProto();
        bool enterOk = result != null;
        clientSession.Send(S_EnterWaitingRoom(roomInfo, enterOk));
    }

    void HandleCChat(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_Chat packet = (C_Chat)message;
        LogHandler.Log(LogCode.CONSOLE, $"Chat: {packet.Chat}");

        RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, S_Chat(session.SUID, packet.Chat));
    }

    #endregion
}
