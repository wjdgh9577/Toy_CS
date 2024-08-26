using CoreLibrary.Job;
using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session;

public class ClientSession : SessionBase
{
    HashSet<RoomBase> _roomList = new HashSet<RoomBase>();

    #region Network

    public override void OnConnected()
    {
        // TODO: 메모리에 유저 데이터 적재
        LogHandler.Log(LogCode.CONSOLE, $"Connected: {SUID}");

        S_Connected packet = new S_Connected();
        packet.ServerTime = Timestamp.FromDateTime(DateTime.UtcNow);
        Send(packet);

        _pingJob = JobTimerHandler.PushAfter(() =>
        {
            SendPing();
        }, Define.PING_INTERVAL);
        _lastPingTime = Environment.TickCount64;
    }

    public override void OnDisconnected()
    {
        // TODO: 메모리에서 유저 관련 데이터 정리
        LogHandler.Log(LogCode.CONSOLE, $"Disconnected: {SUID}");

        _pingJob.Cancel = true;
        RoomManager.Instance.LeaveRoom(this, _roomList);
        SessionManager.Instance.Remove(this);
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

    long _lastPingTime;
    IJob? _pingJob;

    void SendPing()
    {
        if (Environment.TickCount64 - _lastPingTime >= Define.PING_TIMEOUT)
        {
            Disconnect();
            return;
        }

        Send(PacketHandler.S_Ping(DateTime.UtcNow));

        _pingJob = JobTimerHandler.PushAfter(() =>
        {
            SendPing();
        }, Define.PING_INTERVAL);
    }

    public void RecvPing()
    {
        _lastPingTime = Environment.TickCount64;
    }

    #endregion

    #region content

    public void EnterRoom(RoomBase room)
    {
        _roomList.Add(room);
    }

    public void LeaveRoom(RoomBase room)
    {
        _roomList.Remove(room);
    }

    #endregion
}
