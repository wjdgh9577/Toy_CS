using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient.Session;

public class ServerSession : SessionBase
{
    const int TICKS_TO_MILLISECONDS = 10000;

    public DateTime ServerTime => DateTime.UtcNow - new TimeSpan(ping * TICKS_TO_MILLISECONDS);
    long ping;

    public override void OnConnected()
    {
        LogHandler.Log(LogCode.CONSOLE, "Connected");

        Send(PacketHandler.C_Connected(Token));

        TestChatProcess();
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
        ArraySegment<byte> packet = PacketHandler.Serialize(message);

        // TODO: 최적화 고려
        Send(packet);
    }

    public void OnPing(DateTime serverTime)
    {
        DateTime localTime = DateTime.UtcNow;
        ping = localTime.Subtract(serverTime).Ticks / TICKS_TO_MILLISECONDS;

        Send(PacketHandler.C_Ping());
    }

    void TestChatProcess()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var command = Console.ReadLine();

                var array = command.Split(' ');

                if (array.Length >= 3 
                    && string.Equals(array[0]?.ToLower(), "enter") 
                    && int.TryParse(array[1], out int id))
                {
                    SessionManager.Instance.Session.Send(PacketHandler.C_EnterWaitingRoom(id, array[2]));
                }
                else if (array.Length >= 2
                    && string.Equals(array[0]?.ToLower(), "leave") 
                    && int.TryParse(array[1], out id))
                {
                    SessionManager.Instance.Session.Send(PacketHandler.C_LeaveWaitingRoom(id));
                }
                else if (array.Length >= 1 
                    && string.Equals(array[0]?.ToLower(), "refresh"))
                {
                    SessionManager.Instance.Session.Send(PacketHandler.C_RefreshWaitingRoom());
                }
                else if (array.Length >= 1 
                    && string.Equals(array[0]?.ToLower(), "quick"))
                {
                    SessionManager.Instance.Session.Send(PacketHandler.C_QuickEnterWaitingRoom());
                }
                else if (array.Length >= 5 
                    && string.Equals(array[0]?.ToLower(), "create")
                    && int.TryParse(array[1], out int type)
                    && int.TryParse(array[2], out int max))
                {
                    SessionManager.Instance.Session.Send(PacketHandler.C_CreateWaitingRoom(type, max, array[3], array[4]));
                }
                else
                {
                    SessionManager.Instance.Session.Send(PacketHandler.C_Chat(command));
                }
            }
        });
    }
}
