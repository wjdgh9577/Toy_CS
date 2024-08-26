using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Data;

public abstract class RoomInfo
{
    public int uniqueId;
    public int type;
    public int personnel;
    public int maxPersonnel;

    public RoomInfo(int uniqueId, int type, int maxPersonnel)
    {
        this.uniqueId = uniqueId;
        this.type = type;
        this.maxPersonnel = maxPersonnel;
        personnel = 0;
    }

    public virtual void Enter()
    {
        personnel += 1;
    }

    public virtual void Leave()
    {
        personnel = Math.Max(personnel - 1, 0);
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
