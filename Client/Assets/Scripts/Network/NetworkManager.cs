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
                    _session = new ServerSession();
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

    public void Send(IMessage message)
    {
        _session.Send(message);
    }

    #endregion
}
