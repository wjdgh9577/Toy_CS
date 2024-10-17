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
    void HandleLogic(Action<SessionBase, IMessage> handler, SessionBase session, string token, IMessage message)
    {
        if (message is S_Connected)
            session.Token = token;
        else if (session.Verify(token) == false)
        {
            LogHandler.LogError(LogCode.PACKET_INVALID_TOKEN, token);
            return;
        }

        Managers.Instance.NetworkManager.Push(handler, session, message);
    }

    #region Handler

    void HandleSConnected(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Connected packet = (S_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSConnected");

        Managers.Instance.NetworkManager.Send(PacketHandler.C_Connected());
    }

    void HandleSAccountInfo(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_AccountInfo packet = (S_AccountInfo)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSAccountInfo", packet.ToString());

        Managers.Instance.GameManager.SetAccountInfo(packet.Info);
    }

    void HandleSPing(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Ping packet = (S_Ping)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSPing", packet.ServerTime.ToDateTime());

        Managers.Instance.NetworkManager.OnPing(packet.ServerTime.ToDateTime());
    }

    void HandleSEnterWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_EnterWaitingRoom packet = (S_EnterWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSEnterRoom", packet.EnterOk, packet.RoomInfo?.BaseInfo.UniqueId);

        if (packet.EnterOk)
        {
            var roomInfo = packet.RoomInfo.ToLocalData();
            Managers.Instance.GameManager.SetWaitingRoomInfo(roomInfo);
            Managers.Instance.UIManager.GetUI<UILobby>().Hide();
            Managers.Instance.SceneManager.LoadSceneAsync(SceneName.WaitingRoomScene, () =>
            {
                Managers.Instance.UIManager.GetUI<UIWaitingRoom>().Show(roomInfo);
            });
        }
    }

    void HandleSLeaveWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_LeaveWaitingRoom packet = (S_LeaveWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSLeaveRoom", packet.LeaveOk);

        if (packet.LeaveOk)
        {
            Managers.Instance.GameManager.SetWaitingRoomInfo(null);
            Managers.Instance.UIManager.GetUI<UIWaitingRoom>().Hide();
            Managers.Instance.SceneManager.LoadSceneAsync(SceneName.LobbyScene, () =>
            {
                Managers.Instance.UIManager.GetUI<UILobby>().Show();
            });
        }
    }

    void HandleSRefreshWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_RefreshWaitingRoom packet = (S_RefreshWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSRefreshRoom", packet.ToString());

        Managers.Instance.GameManager.SetWaitingRoomInfo(packet.RoomInfo.ToLocalData());
        Managers.Instance.UIManager.GetUI<UIWaitingRoom>().OnRefresh();
    }

    void HandleSReadyWaitingRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_ReadyWaitingRoom packet = (S_ReadyWaitingRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSReadyWaitingRoom", packet.ToString());

        Managers.Instance.UIManager.GetUI<UIWaitingRoom>().OnRefresh(packet.Info.ToLocalData());
    }

    void HandleSRefreshLobby(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_RefreshLobby packet = (S_RefreshLobby)message;
        LogHandler.Log(LogCode.CONSOLE, "HandleSRefreshLobby", packet.ToString());

        Managers.Instance.UIManager.GetUI<UILobby>().OnRefresh(packet.RoomInfos.ToLocalData());
    }

    void HandleSChat(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_Chat packet = (S_Chat)message;
        LogHandler.Log(LogCode.CONSOLE, $"From: Session_{packet.Info.Name}, Chat: {packet.Chat}");

        Managers.Instance.UIManager.GetUI<UIWaitingRoom>().OnChat(packet.Info.ToLocalData(), packet.Chat);
    }

    void HandleSEnterGameRoom(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_EnterGameRoom packet = (S_EnterGameRoom)message;
        LogHandler.Log(LogCode.CONSOLE, "S_EnterGameRoom", packet.ToString());

        Managers.Instance.GameManager.SetGameRoomInfo(packet.RoomInfo.ToLocalData());
        Managers.Instance.SceneManager.LoadSceneAsync(SceneName.GameScene, () =>
        {
            Managers.Instance.UIManager.GetUI<UIWaitingRoom>().Hide();
            Managers.Instance.NetworkManager.Send(PacketHandler.C_EnterGameRoom(Managers.Instance.GameManager.MyGameRoomPlayerInfo));
        });
    }

    void HandleSStartGame(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_StartGame packet = (S_StartGame)message;
        LogHandler.Log(LogCode.CONSOLE, "S_StartGame", packet.ToString());

        // TODO: 게임 시작
    }

    void HandleSSyncPlayer(SessionBase session, IMessage message)
    {
        ServerSession serverSession = (ServerSession)session;
        S_SyncPlayer packet = (S_SyncPlayer)message;
        LogHandler.Log(LogCode.CONSOLE, $"HandleSSyncPlayer", packet.ToString());

        Managers.Instance.GameManager.UpdatePlayers(packet.Infos.ToLocalData());
    }

    #endregion
}
