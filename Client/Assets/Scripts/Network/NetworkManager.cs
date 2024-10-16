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

    public ServerSession Session { get; private set; }

    public void Connect(Action<bool> connectCallback)
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
                    Session = new ServerSession();
                    Session.Start(connectSocket);
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
                LogHandler.Log(LogCode.CONSOLE, "��õ�");
                Push(() => connectCallback.Invoke(false));
            }

            connector.Connected -= OnConnected;
        }
    }

    public void Disconnect()
    {
        Session?.Disconnect();
    }

    #endregion

    #region Packet

    public void Send(IMessage message)
    {
        Session.Send(message);
    }

    #endregion

    const int TICKS_TO_MILLISECONDS = 10000;

    public DateTime ServerTime => DateTime.UtcNow - new TimeSpan(ping * TICKS_TO_MILLISECONDS);
    public long ping { get; private set; }

    public void OnPing(DateTime serverTime)
    {
        DateTime localTime = DateTime.UtcNow;
        ping = localTime.Subtract(serverTime).Ticks / TICKS_TO_MILLISECONDS;

        Send(PacketHandler.C_Ping());
    }
}
