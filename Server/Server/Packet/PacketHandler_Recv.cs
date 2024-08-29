using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Content.Room;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    void HandleLogic(Action<SessionBase, IMessage> handler, SessionBase session, string token, IMessage message)
    {
        if (session.Verify(token) == false)
        {
            LogHandler.LogError(LogCode.PACKET_INVALID_TOKEN, token);
            return;
        }
        handler.Invoke(session, message);
    }

    #region Handler

    void HandleCConnected(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_Connected packet = (C_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCConnected");

        clientSession.StartPing();

        // TODO: DB로부터 AccountInfo 긁어오기
        clientSession.SetAccountInfo(); // 임시값
        clientSession.Send(S_AccountInfo(clientSession.AccountInfo.GetProto()));
    }

    void HandleCPing(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_Ping packet = (C_Ping)message;
        //LogHandler.Log(LogCode.CONSOLE, "HandleCPing");

        clientSession.RecvPing();
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

        if (enterOk)
        {
            // 방 전체 broadcast
            RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, S_RefreshWaitingRoom(roomInfo));
        }
    }

    void HandleCLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_LeaveWaitingRoom packet = (C_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCLeaveRoom", packet.ToString());

        WaitingRoom? result = RoomManager.Instance.LeaveRoom<WaitingRoom>(clientSession, packet.UniqueId);
        bool leaveOk = result != null;
        clientSession.Send(S_LeaveWaitingRoom(leaveOk));

        if (leaveOk)
        {
            // 방 전체 broadcast
            RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, packet.UniqueId, S_RefreshWaitingRoom(result.Info.GetProto()));
        }
    }

    void HandleCReadyWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_ReadyWaitingRoom packet = (C_ReadyWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCReadyWaitingRoom", packet.ToString());

        clientSession.AccountInfo.Ready = packet.Ready;
        RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, S_ReadyWaitingRoom(clientSession.AccountInfo.GetProto()));
    }

    void HandleCRefreshLobby(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_RefreshLobby packet = (C_RefreshLobby)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCRefreshRoom");

        List<WaitingRoom> roomList = RoomManager.Instance.GetRooms<WaitingRoom>();
        clientSession.Send(S_RefreshLobby(roomList));
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

        if (enterOk)
        {
            // 방 전체 broadcast
            RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, S_RefreshWaitingRoom(roomInfo));
        }
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

        RoomManager.Instance.Broadcast<WaitingRoom>(clientSession, S_Chat(clientSession.AccountInfo.GetProto(), packet.Chat));
    }

    #endregion
}
