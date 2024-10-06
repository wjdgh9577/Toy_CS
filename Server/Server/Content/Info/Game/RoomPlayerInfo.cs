using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server.Content.Info;

public abstract class RoomPlayerInfo
{
    public AccountInfo AccountInfo { get; set; }

    protected Google.Protobuf.Protocol.RoomPlayerInfo GetProto()
    {
        var info = new Google.Protobuf.Protocol.RoomPlayerInfo();
        info.AccountInfo = AccountInfo.GetProto();

        return info;
    }
}

public class WaitingRoomPlayerInfo : RoomPlayerInfo
{
    public bool Ready { get; set; }

    public WaitingRoomPlayerInfo(AccountInfo info)
    {
        AccountInfo = info;
        Ready = false;
    }

    public new Google.Protobuf.Protocol.WaitingRoomPlayerInfo GetProto()
    {
        Google.Protobuf.Protocol.WaitingRoomPlayerInfo info = new Google.Protobuf.Protocol.WaitingRoomPlayerInfo();
        info.BaseInfo = base.GetProto();
        info.Ready = Ready;

        return info;
    }
}

public class GameRoomPlayerInfo : RoomPlayerInfo
{
    [Flags]
    public enum PlayerState : byte
    {
        // System
        LOADING = 0b00000001,
        WAITING = 0b00000010,
        PLAYING = 0b00000100,

        // Behavior
        IDLE = 0b00010000,
        WALK = 0b00100000,
        JUMP = 0b01000000,
    }

    PlayerState _state;
    public PlayerState State
    {
        get => _state;
        set
        {
            _isDirty = true;
            _state = value;
        }
    }

    bool _isDirty;

    public GameRoomPlayerInfo(AccountInfo info)
    {
        _state = PlayerState.LOADING;
        _isDirty = false;
    }
}
