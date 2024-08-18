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

public class GameSession : SessionBase
{
    const int PING_INTERVAL = 30000;
    IJob pingJob = null;

    HashSet<RoomBase> _roomList = new HashSet<RoomBase>();

    #region Network

    public override void OnConnected()
    {
        // TODO: 메모리에 유저 데이터 적재
        LogHandler.Log(LogCode.CONSOLE, $"Connected: {SUID}");

        S_Connected packet = new S_Connected();
        packet.ServerTime = Timestamp.FromDateTime(DateTime.UtcNow);
        Send(packet);

        Ping();
    }

    public override void OnDisconnected()
    {
        // TODO: 메모리에서 유저 관련 데이터 정리
        LogHandler.Log(LogCode.CONSOLE, $"Disconnected: {SUID}");

        pingJob.Cancel = true;
        RoomManager.Instance.LeaveRoom(this, _roomList);
        SessionManager.Instance.Remove(this);
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

    void Ping()
    {
        S_Ping packet = new S_Ping();
        packet.ServerTime = Timestamp.FromDateTime(DateTime.UtcNow);
        Send(packet);

        pingJob = JobTimerHandler.PushAfter(() =>
        {
            Ping();
        }, PING_INTERVAL);
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
