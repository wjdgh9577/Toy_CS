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

        RoomManager.Instance.Handle(() =>
        {
            WaitingRoom? result = RoomManager.Instance.EnterRoom<WaitingRoom>(clientSession, packet.UniqueId, packet.Password);
            WaitingRoomInfo? roomInfo = result?.Info.GetProto();
            bool enterOk = result != null;
            clientSession.Send(S_EnterWaitingRoom(roomInfo, enterOk));
        });
    }

    void HandleCLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_LeaveWaitingRoom packet = (C_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCLeaveRoom", packet.ToString());

        RoomManager.Instance.Handle(() =>
        {
            WaitingRoom? result = RoomManager.Instance.LeaveRoom<WaitingRoom>(clientSession, packet.UniqueId);
            bool leaveOk = result != null;
            clientSession.Send(S_LeaveWaitingRoom(leaveOk));
        });
    }

    void HandleCReadyWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_ReadyWaitingRoom packet = (C_ReadyWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCReadyWaitingRoom", packet.ToString());

        RoomManager.Instance.Handle(() =>
        {
            WaitingRoom? room = RoomManager.Instance.FindRoom<WaitingRoom>(clientSession);

            bool canStart = true;
            foreach (var p in room.Info.players.Values)
            {
                if (p.AccountInfo.Uuid == clientSession.AccountInfo.Uuid)
                {
                    p.Ready = packet.Ready;
                }
                if (!p.Ready)
                {
                    canStart = false;
                }
            }

            if (canStart && room.Info.players.Count > 1)
            {
                room.StartGame();
            }
            else
            {
                room.Broadcast(clientSession, S_ReadyWaitingRoom(clientSession.AccountInfo.GetProto(), packet.Ready));
            }
        });
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

        RoomManager.Instance.Handle(() =>
        {
            WaitingRoom? result = RoomManager.Instance.QuickWaitingRoom(clientSession);
            WaitingRoomInfo? roomInfo = result?.Info.GetProto();
            bool enterOk = result != null;
            clientSession.Send(S_EnterWaitingRoom(roomInfo, enterOk));
        });
    }

    void HandleCCreateWaitingRoom(SessionBase session, IMessage message)
    {
        ClientSession clientSession = (ClientSession)session;
        C_CreateWaitingRoom packet = (C_CreateWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleCCreateRoom", packet.ToString());

        RoomManager.Instance.Handle(() =>
        {
            WaitingRoom? result = RoomManager.Instance.CreateRoom<WaitingRoom>(clientSession, packet.Type, packet.MaxPersonnel, (newRoom) =>
            {
                newRoom.Info.title = packet.Title;
                newRoom.Info.password = packet.Password;
            });
            WaitingRoomInfo? roomInfo = result?.Info.GetProto();
            bool enterOk = result != null;
            clientSession.Send(S_EnterWaitingRoom(roomInfo, enterOk));
        });
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
