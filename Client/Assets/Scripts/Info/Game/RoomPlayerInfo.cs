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
    public GameRoomPlayerInfo(Google.Protobuf.Protocol.GameRoomPlayerInfo info) : base (info.BaseInfo)
    {
        
    }

    public new Google.Protobuf.Protocol.GameRoomPlayerInfo GetProto()
    {
        var info = new Google.Protobuf.Protocol.GameRoomPlayerInfo()
        {
            BaseInfo = base.GetProto(),
        };

        return info;
    }
}