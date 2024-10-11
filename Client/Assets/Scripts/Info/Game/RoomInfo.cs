using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RoomInfo : IInfo<int>
{
    public int uniqueId;
    public int type;
    public int personnel;
    public int maxPersonnel;
    public int mapId;

    protected RoomInfo(Google.Protobuf.Protocol.RoomInfo info)
    {
        uniqueId = info.UniqueId;
        type = info.Type;
        personnel = info.Personnel;
        maxPersonnel = info.MaxPersonnel;
        mapId = info.MapId;
    }

    public int GetKey() => uniqueId;
}

public sealed class WaitingRoomInfo : RoomInfo
{
    public string title;
    public bool password;
    public List<WaitingRoomPlayerInfo> players;

    public WaitingRoomInfo(Google.Protobuf.Protocol.WaitingRoomInfo info) : base(info.BaseInfo)
    {
        title = info.Title;
        password = info.Password;
        players = info.Players.ToLocalData();
    }
}

public sealed class GameRoomInfo : RoomInfo
{
    public List<GameRoomPlayerInfo> players;

    public GameRoomInfo(Google.Protobuf.Protocol.GameRoomInfo info) : base(info.BaseInfo)
    {
        players = info.Players.ToLocalData();
    }
}
