﻿using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
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

        C_EnterRoom packet = new C_EnterRoom();
        packet.RoomId = 1;
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
        LogHandler.Log(LogCode.CONSOLE, $"BytesTransferred: {BytesTransferred}");
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
    }

    void TestChatProcess()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var chat = Console.ReadLine();
                C_Chat packet = new C_Chat();
                packet.Chat = chat;
                Send(packet);
            }
        });
    }
}
