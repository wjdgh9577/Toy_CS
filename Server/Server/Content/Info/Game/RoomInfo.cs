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
    public int mapId;

    public RoomInfo(int uniqueId, int type, int maxPersonnel)
    {
        this.uniqueId = uniqueId;
        this.type = type;
        this.personnel = 0;
        this.maxPersonnel = maxPersonnel;
        this.mapId = 1;
    }

    public virtual void Enter(AccountInfo info)
    {
        
    }

    public virtual void Leave(AccountInfo info)
    {
        
    }

    public override string ToString()
    {
        return $"Unique ID: {uniqueId}, ID: {type}";
    }

    protected Google.Protobuf.Protocol.RoomInfo GetProto()
    {
        var baseInfo = new Google.Protobuf.Protocol.RoomInfo();

        baseInfo.UniqueId = uniqueId;
        baseInfo.Type = type;
        baseInfo.Personnel = personnel;
        baseInfo.MaxPersonnel = maxPersonnel;

        return baseInfo;
    }
}

public sealed class WaitingRoomInfo : RoomInfo
{
    public string title;
    public string password;

    public Dictionary<string, WaitingRoomPlayerInfo> players;

    public WaitingRoomInfo(int uniqueId, int type, int maxPersonnel) : base(uniqueId, type, maxPersonnel)
    {
        this.players = new Dictionary<string, WaitingRoomPlayerInfo>();
    }

    public override void Enter(AccountInfo info)
    {
        base.Enter(info);

        players.Add(info.Uuid, new WaitingRoomPlayerInfo(info));
        personnel = players.Count;
    }

    public override void Leave(AccountInfo info)
    {
        base.Leave(info);

        players.Remove(info.Uuid);
        personnel = players.Count;
    }

    public new Google.Protobuf.Protocol.WaitingRoomInfo GetProto()
    {
        Google.Protobuf.Protocol.WaitingRoomInfo info = new Google.Protobuf.Protocol.WaitingRoomInfo();
        info.BaseInfo = base.GetProto();
        info.Title = title;
        info.Password = !string.IsNullOrEmpty(password);
        foreach (var p in players)
            info.Players.Add(p.Value.GetProto());

        return info;
    }

    public bool Verification(string password)
    {
        return string.Equals(this.password, password);
    }
}

public sealed class GameRoomInfo : RoomInfo
{
    public Dictionary<string, GameRoomPlayerInfo> players;

    public GameRoomInfo(int uniqueId, int type, int maxPersonnel) : base(uniqueId, type, maxPersonnel)
    {
        this.players = new Dictionary<string, GameRoomPlayerInfo>();
    }

    public override void Enter(AccountInfo info)
    {
        base.Enter(info);

        players.Add(info.Uuid, new GameRoomPlayerInfo(info));
        personnel = players.Count;
    }

    public override void Leave(AccountInfo info)
    {
        base.Leave(info);

        players.Remove(info.Uuid);
        personnel = players.Count;
    }
}
