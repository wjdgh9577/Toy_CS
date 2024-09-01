using CoreLibrary.Job;
using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Server.Content.Data;
using Server.Content.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session;

public class ClientSession : SessionBase
{
    #region Network

    public override void OnConnected()
    {
        LogHandler.Log(LogCode.CONSOLE, $"Connected: {SUID}");

        Send(PacketHandler.S_Connected());
    }

    public override void OnDisconnected()
    {
        LogHandler.Log(LogCode.CONSOLE, $"Disconnected: {SUID}");

        RoomManager.Instance.LeaveRoom(this, _roomList);
        SessionManager.Instance.Remove(this);

        _pingJob.Cancel = true;
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

        // TODO: 최적화 고려
        Send(packet);
    }

    long _lastPingTime;
    IJob? _pingJob;

    public void StartPing()
    {
        _lastPingTime = Environment.TickCount64;
        SendPing();
    }

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

    public AccountInfo AccountInfo { get; private set; }

    HashSet<RoomBase> _roomList = new HashSet<RoomBase>();

    public void SetAccountInfo(/* DB Model */)
    {
        AccountInfo = new AccountInfo(Token, $"Session_{SUID}", 1); // 임시값
    }

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
