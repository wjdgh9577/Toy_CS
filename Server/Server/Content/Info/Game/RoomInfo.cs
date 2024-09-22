using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Info;

public abstract class RoomInfo
{
    public int uniqueId;
    public int type;
    public int personnel;
    public int maxPersonnel;

    public Dictionary<string, AccountInfo> players;

    public RoomInfo(int uniqueId, int type, int maxPersonnel)
    {
        this.uniqueId = uniqueId;
        this.type = type;
        this.maxPersonnel = maxPersonnel;
        players = new Dictionary<string, AccountInfo>();
        personnel = 0;
    }

    public virtual void Enter(AccountInfo info)
    {
        players.Add(info.Uuid, info);
        personnel = players.Count;
    }

    public virtual void Leave(AccountInfo info)
    {
        players.Remove(info.Uuid);
        personnel = players.Count;
    }

    public override string ToString()
    {
        return $"Unique ID: {uniqueId}, ID: {type}";
    }
}

public sealed class WaitingRoomInfo : RoomInfo
{
    public string title;
    public string password;

    public WaitingRoomInfo(int uniqueId, int type, int maxPersonnel) : base(uniqueId, type, maxPersonnel)
    {

    }

    public Google.Protobuf.Protocol.WaitingRoomInfo GetProto()
    {
        Google.Protobuf.Protocol.WaitingRoomInfo info = new Google.Protobuf.Protocol.WaitingRoomInfo();
        info.BaseInfo = new Google.Protobuf.Protocol.RoomInfo();
        info.BaseInfo.UniqueId = uniqueId;
        info.BaseInfo.Type = type;
        info.BaseInfo.Personnel = personnel;
        info.BaseInfo.MaxPersonnel = maxPersonnel;
        foreach (var p in players)
            info.BaseInfo.Players.Add(p.Value.GetProto());
        info.Title = title;
        info.Password = !string.IsNullOrEmpty(password);

        return info;
    }

    public bool Verification(string password)
    {
        return string.Equals(this.password, password);
    }
}

public sealed class GameRoomInfo : RoomInfo
{
    public GameRoomInfo(int uniqueId, int type, int maxPersonnel) : base(uniqueId, type, maxPersonnel)
    {

    }
}
