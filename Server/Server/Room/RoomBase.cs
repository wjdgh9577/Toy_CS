using CoreLibrary.Network;
using Google.Protobuf;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Room;

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
        this.personnel = 0;
    }

    public virtual void Enter()
    {
        this.personnel += 1;
    }

    public virtual void Leave()
    {
        this.personnel = Math.Max(this.personnel - 1, 0);
    }

    public override string ToString()
    {
        return $"Unique ID: {uniqueId}, ID: {type}";
    }
}

public abstract class RoomBase
{
    protected RoomInfo _info { get; set; }
    public bool Accessible { get { return _info.personnel < _info.maxPersonnel; } }

    protected Dictionary<int, ClientSession> _sessions { get; } = new Dictionary<int, ClientSession>();

    public abstract void OnStart(int uniqueId, int type, int maxPersonnel);

    public virtual void OnUpdate() { }

    public virtual void OnDestroy() { }

    public virtual void OnEnter(ClientSession session)
    {
        _info.Enter();
    }

    public virtual void OnLeave(ClientSession session)
    {
        _info.Leave();
    }

    public virtual void Broadcast(ClientSession session, IMessage message)
    {
        foreach (var _session in _sessions.Values)
        {
            if (_session == session)
                continue;
            _session.Send(message);
        }
    }

    public bool TryGetSession(int suid, out ClientSession? session)
    {
        return _sessions.TryGetValue(suid, out session);
    }

    public bool ContainsSession(int suid)
    {
        return _sessions.ContainsKey(suid);
    }
}
