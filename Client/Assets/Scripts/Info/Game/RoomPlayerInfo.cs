using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomPlayerInfo : IInfo<string>
{
    public AccountInfo accountInfo;

    protected RoomPlayerInfo(Google.Protobuf.Protocol.RoomPlayerInfo info)
    {
        accountInfo = info.AccountInfo.ToLocalData();
    }

    protected Google.Protobuf.Protocol.RoomPlayerInfo GetProto()
    {
        var baseInfo = new Google.Protobuf.Protocol.RoomPlayerInfo()
        {
            AccountInfo = accountInfo.GetProto()
        };

        return baseInfo;
    }

    public string GetKey() => accountInfo.Uuid;
}

public class WaitingRoomPlayerInfo : RoomPlayerInfo
{
    public bool ready;

    public WaitingRoomPlayerInfo(Google.Protobuf.Protocol.WaitingRoomPlayerInfo info) : base(info.BaseInfo)
    {
        ready = info.Ready;
    }
}

public class GameRoomPlayerInfo : RoomPlayerInfo
{
    Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
                IsDirty = true;
            _position = value;
        }
    }

    public bool IsDirty { get; private set; } = false;

    public Player Player { get; set; }

    public GameRoomPlayerInfo(Google.Protobuf.Protocol.GameRoomPlayerInfo info) : base (info.BaseInfo)
    {
        _position = new Vector2(info.Transform.XPos, info.Transform.YPos);
    }

    public new Google.Protobuf.Protocol.GameRoomPlayerInfo GetProto()
    {
        var info = new Google.Protobuf.Protocol.GameRoomPlayerInfo()
        {
            BaseInfo = base.GetProto(),
            Transform = new Google.Protobuf.Protocol.Transform()
            {
                XPos = Position.x,
                YPos = Position.y
            }
        };

        IsDirty = false;

        return info;
    }
}