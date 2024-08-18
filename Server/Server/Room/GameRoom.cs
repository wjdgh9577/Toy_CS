using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Room;

public class GameRoom : RoomBase
{
    Dictionary<int, GameSession> _sessions = new Dictionary<int, GameSession>();

    public override void OnStart(RoomInfo info)
    {
        base.OnStart(info);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Info.ccu == 0)
        {
            RoomManager.Instance.DestroyRoom<GameRoom>(Info.id);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnEnter(GameSession session)
    {
        base.OnEnter(session);

        if (_sessions.TryAdd(session.SUID, session) == false)
            LogHandler.LogError(LogCode.ROOM_SESSION_INVALID_UID, $"SUID ({session.SUID}) is already used.");
    }

    public override void OnLeave(GameSession session)
    {
        base.OnLeave(session);

        if (_sessions.Remove(session.SUID) == false)
            LogHandler.LogError(LogCode.ROOM_SESSION_NOT_EXIST, $"Session_{session.SUID} is not exist.");
    }

    public void Broadcast(IMessage message)
    {
        foreach (var session in _sessions.Values)
        {
            session.Send(message);
        }
    }
}
