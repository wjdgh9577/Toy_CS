using CoreLibrary.Job;
using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class NetworkManager : JobSerializer
{
    #region Singleton

    int _instantiated = 0;
    public NetworkManager()
    {
        if (Interlocked.Exchange(ref _instantiated, 1) == 1)
        {
            LogHandler.LogError(LogCode.EXCEPTION, $"{this} is already instantiated.");
            throw new Exception();
        }
    }

    #endregion

    public void Start()
    {
        
    }

    public override void Update()
    {
        Flush();
    }

    #region Session

    ServerSession _session;

    public void Connect(string token, Action<bool> connectCallback)
    {
        NetworkHandler.TcpConnector(out var connector);
        connector.Connected += OnConnected;

        connector.Start();

        void OnConnected(bool connected, SocketAsyncEventArgs args)
        {
            if (connected)
            {
                Socket connectSocket = args.ConnectSocket;
                if (connectSocket != null)
                {
                    _session = new ServerSession();
                    _session.Token = token;
                    _session.Start(connectSocket);
                    Push(() => connectCallback.Invoke(true));
                }
                else
                {
                    LogHandler.LogError(LogCode.SOCKET_ERROR, "Missing connect socket.");
                    Push(() => connectCallback.Invoke(false));
                }
            }
            else
            {
                LogHandler.Log(LogCode.CONSOLE, "Àç½Ãµµ");
                Push(() => connectCallback.Invoke(false));
            }

            connector.Connected -= OnConnected;
        }
    }

    public void Disconnect()
    {
        _session.Disconnect();
    }

    #endregion

    #region Packet

    void Send(IMessage message)
    {
        _session.Send(message);
    }

    public void SendCPing()
    {
        C_Ping packet = new C_Ping();
        Send(packet);
    }

    public void SendCConnected(string token)
    {
        C_Connected packet = new C_Connected();
        packet.Token = token;
        Send(packet);
    }

    public void SendCEnterWaitingRoom(int uniqueId, string password)
    {
        C_EnterWaitingRoom packet = new C_EnterWaitingRoom();
        packet.UniqueId = uniqueId;
        packet.Password = password;
        Send(packet);
    }

    public void SendCLeaveWaitingRoom(int uniqueId)
    {
        C_LeaveWaitingRoom packet = new C_LeaveWaitingRoom();
        packet.UniqueId = uniqueId;
        Send(packet);
    }

    public void SendCRefreshWaitingRoom()
    {
        C_RefreshWaitingRoom packet = new C_RefreshWaitingRoom();
        Send(packet);
    }

    public void SendCQuickEnterWaitingRoom()
    {
        C_QuickEnterWaitingRoom packet = new C_QuickEnterWaitingRoom();
        Send(packet);
    }

    public void SendCCreateWaitingRoom(int type, int maxPersonnel, string title, string password)
    {
        C_CreateWaitingRoom packet = new C_CreateWaitingRoom();
        packet.Type = type;
        packet.MaxPersonnel = maxPersonnel;
        packet.Title = title;
        packet.Password = password;
        Send(packet);
    }

    public void SendCChat(string chat)
    {
        C_Chat packet = new C_Chat();
        packet.Chat = chat;
        Send(packet);
    }

    #endregion
}
