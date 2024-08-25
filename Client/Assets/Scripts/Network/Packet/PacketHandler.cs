using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    // 메인스레드 로직
    void HandleLogic(Action<SessionBase, IMessage> handler, SessionBase session, IMessage message)
    {
        Managers.Instance.NetworkManager.Push(handler, session, message);
    }

    #region Handler

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

        if (packet.EnterOk)
        {
            // TODO: 대기방 이동
        }
    }

    void HandleSLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_LeaveWaitingRoom packet = (S_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSLeaveRoom", packet.LeaveOk);

        if (packet.LeaveOk)
        {
            // TODO: 로비씬 이동
        }
    }

    void HandleSRefreshWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_RefreshWaitingRoom packet = (S_RefreshWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSRefreshRoom", packet.ToString());

        Managers.Instance.UIManager.GetUI<UILobby>().OnRefresh(packet.RoomInfos);
    }

    void HandleSChat(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Chat packet = (S_Chat)message;
        LogHandler.Log(LogCode.CONSOLE, $"From: Session_{packet.Suid}, Chat: {packet.Chat}");
    }

    #endregion
}
