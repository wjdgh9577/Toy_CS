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
    public List<AccountInfo> players;

    protected RoomInfo(Google.Protobuf.Protocol.RoomInfo info)
    {
        uniqueId = info.UniqueId;
        type = info.Type;
        personnel = info.Personnel;
        maxPersonnel = info.MaxPersonnel;
        mapId = info.MapId;
        players = info.Players.ToLocalData();
    }

    public int GetKey() => uniqueId;
}

public sealed class WaitingRoomInfo : RoomInfo
{
    public string title;
    public bool password;
    public AccountInfo chief;

    public WaitingRoomInfo(Google.Protobuf.Protocol.WaitingRoomInfo info) : base(info.BaseInfo)
    {
        title = info.Title;
        password = info.Password;
        chief = info.Chief.ToLocalData();
    }
}

public sealed class GameRoomInfo : RoomInfo
{
    public GameRoomInfo() : base(null)
    {

    }
}
