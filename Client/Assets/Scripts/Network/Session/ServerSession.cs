using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServerSession : SessionBase
{
    const int TICKS_TO_MILLISECONDS = 10000;

    public DateTime ServerTime => DateTime.UtcNow - new TimeSpan(ping * TICKS_TO_MILLISECONDS);
    long ping;

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
        PacketHandler.HandlePacket(this, buffer);
    }

    public override void OnSend(int BytesTransferred)
    {
        //LogHandler.Log(LogCode.CONSOLE, $"BytesTransferred: {BytesTransferred}");
    }

    public void Send(IMessage message)
    {
        ArraySegment<byte> packet = PacketHandler.Serialize(Token, message);

        Send(packet);
    }

    public void OnPing(DateTime serverTime)
    {
        DateTime localTime = DateTime.UtcNow;
        ping = localTime.Subtract(serverTime).Ticks / TICKS_TO_MILLISECONDS;

        Managers.Instance.NetworkManager.Send(PacketHandler.C_Ping());
    }
}