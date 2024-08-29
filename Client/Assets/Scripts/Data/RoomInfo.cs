using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RoomInfo
{
    public int uniqueId;
    public int type;
    public int personnel;
    public int maxPersonnel;
    public List<AccountInfo> players;
}

public sealed class WaitingRoomInfo : RoomInfo
{
    public string title;
    public bool password;

    public WaitingRoomInfo(Google.Protobuf.Protocol.WaitingRoomInfo info)
    {
        uniqueId = info.BaseInfo.UniqueId;
        type = info.BaseInfo.Type;
        personnel = info.BaseInfo.Personnel;
        maxPersonnel = info.BaseInfo.MaxPersonnel;
        players = info.BaseInfo.Players.ToLocalData();

        title = info.Title;
        password = info.Password;
    }
}

public sealed class GameRoomInfo : RoomInfo
{
    public GameRoomInfo()
    {

    }
}
