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

        C_Connected packet = new C_Connected();
        packet.Token = Token;
        Send(packet);

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

        C_Ping resPacket = new C_Ping();
        Send(resPacket);
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
                    C_EnterWaitingRoom packet = new C_EnterWaitingRoom();
                    packet.UniqueId = id;
                    packet.Password = array[2];
                    Send(packet);
                }
                else if (array.Length >= 2
                    && string.Equals(array[0]?.ToLower(), "leave") 
                    && int.TryParse(array[1], out id))
                {
                    C_LeaveWaitingRoom packet = new C_LeaveWaitingRoom();
                    packet.UniqueId = id;
                    Send(packet);
                }
                else if (array.Length >= 1 
                    && string.Equals(array[0]?.ToLower(), "refresh"))
                {
                    C_RefreshWaitingRoom packet = new C_RefreshWaitingRoom();
                    Send(packet);
                }
                else if (array.Length >= 1 
                    && string.Equals(array[0]?.ToLower(), "quick"))
                {
                    C_QuickEnterWaitingRoom packet = new C_QuickEnterWaitingRoom();
                    Send(packet);
                }
                else if (array.Length >= 5 
                    && string.Equals(array[0]?.ToLower(), "create")
                    && int.TryParse(array[1], out int type)
                    && int.TryParse(array[2], out int max))
                {
                    C_CreateWaitingRoom packet = new C_CreateWaitingRoom();
                    packet.Type = type;
                    packet.MaxPersonnel = max;
                    packet.Title = array[3];
                    packet.Password = array[4];
                    Send(packet);
                }
                else
                {
                    C_Chat packet = new C_Chat();
                    packet.Chat = command;
                    Send(packet);
                }
            }
        });
    }
}
