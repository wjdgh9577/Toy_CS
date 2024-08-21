using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class NetworkManager : IManger
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

    public void Update()
    {
        Queue<Packet> packets = _packetQueue.DequeueAll();

        while (packets.Count > 0)
            packets.Dequeue().Handle();
    }

    #region Session

    ServerSession _session = new ServerSession();

    public void Connect()
    {
        NetworkHandler.TcpConnector(out var connector);
        connector.Connected += (connected, args) =>
        {
            if (connected)
            {
                Socket connectSocket = args.ConnectSocket;
                if (connectSocket != null)
                {
                    _session.Start(connectSocket);
                }
                else
                {
                    LogHandler.LogError(LogCode.SOCKET_ERROR, "Missing connect socket.");
                }
            }
            else
            {
                // TODO: 팝업창, 재시도
                LogHandler.Log(LogCode.CONSOLE, "재시도");
                //connector.Start();
            }
        };

        connector.Start();
    }

    public void Disconnect()
    {
        _session.Disconnect();
    }

    #endregion

    #region Packet

    PacketQueue _packetQueue = new PacketQueue();

    public void HandlePacket(Action<SessionBase, IMessage> handler, SessionBase session, IMessage message)
    {
        _packetQueue.Enqueue(handler, session, message);
    }

    #endregion
}
