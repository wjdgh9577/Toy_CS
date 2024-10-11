using CoreLibrary.Network;
using Google.Protobuf;
using Server.Content.Info;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Room;

public abstract class RoomBase
{
    protected RoomInfo _info { get; set; }
    public bool Accessible { get { return _info.personnel < _info.maxPersonnel; } }

    protected Dictionary<int, ClientSession> _sessions { get; } = new Dictionary<int, ClientSession>();

    public abstract void OnStart(int uniqueId, int type, int maxPersonnel);

    public virtual void OnUpdate()
    {
        if (_info.personnel == 0)
        {
            RoomManager.Instance.DestroyRoom(_info.uniqueId);
        }
    }

    public virtual void OnDestroy() { }

    public virtual void OnEnter(ClientSession session)
    {
        _info.Enter(session.AccountInfo);
        _sessions.TryAdd(session.SUID, session);
    }

    public virtual void OnLeave(ClientSession session)
    {
        _info.Leave(session.AccountInfo);
        _sessions.Remove(session.SUID);
    }

    public void Broadcast(ClientSession session, IMessage message)
    {
        foreach (var _session in _sessions.Values)
        {
            if (_session == session)
                continue;

            _session.Send(message);
        }
    }

    public void Broadcast(IMessage message)
    {
        foreach (var _session in _sessions.Values)
        {
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
