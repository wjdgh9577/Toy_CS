using CoreLibrary.Log;
using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient.Session;

public class GameSession : TcpSession
{
    public override void OnConnected()
    {
        LogHandler.Log(LogCode.CONSOLE, "Connected");
    }

    public override void OnDisconnected()
    {
        LogHandler.Log(LogCode.CONSOLE, "Disconnected");
    }

    public override void OnRecv(ArraySegment<byte> buffer)
    {
        
    }

    public override void OnSend(int BytesTransferred)
    {
        
    }
}
