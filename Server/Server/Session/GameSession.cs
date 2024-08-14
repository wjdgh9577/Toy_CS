﻿using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session;

public class GameSession : TcpSession
{
    public override void OnConnected()
    {
        // TODO: 메모리에 유저 데이터 적재
        LogHandler.Log(LogCode.CONSOLE, "Connected");

        S_Connected packet = new S_Connected();
        packet.ServerTime = Timestamp.FromDateTime(DateTime.UtcNow);
        Send(packet);
    }

    public override void OnDisconnected()
    {
        // TODO: 메모리에서 유저 관련 데이터 정리
        LogHandler.Log(LogCode.CONSOLE, "Disconnected");
    }

    public override void OnRecv(ArraySegment<byte> buffer)
    {
        PacketHandler.HandlePacket(this, buffer);
    }

    public override void OnSend(int BytesTransferred)
    {
        LogHandler.Log(LogCode.CONSOLE, BytesTransferred);
    }

    public void Send(IMessage message)
    {
        ArraySegment<byte> packet = PacketHandler.Serialize(message);

        // TODO: 최적화 고려
        Send(packet);
    }
}
