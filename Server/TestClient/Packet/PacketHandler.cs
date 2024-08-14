using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Session;

public partial class PacketHandler
{
    void HandleSPing(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_Ping packet = (S_Ping)message;
        LogHandler.Log(LogCode.CONSOLE, packet.ServerTime);

        gameSession.OnPing(packet.ServerTime.ToDateTime());
    }

    void HandleSConnected(SessionBase session, IMessage message)
    {
        GameSession gameSession = (GameSession)session;
        S_Connected packet = (S_Connected)message;
        LogHandler.Log(LogCode.CONSOLE, packet.ServerTime.ToDateTime());

        gameSession.OnPing(packet.ServerTime.ToDateTime());
    }
}
