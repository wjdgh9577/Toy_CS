using CoreLibrary.Log;
using Google.Protobuf;
using Server.Content.Data;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Room;

public class WaitingRoom : RoomBase
{
    public WaitingRoomInfo Info { get { return (WaitingRoomInfo)_info; } }

    public override void OnStart(int uniqueId, int type, int maxPersonnel)
    {
        _info = new WaitingRoomInfo(uniqueId, type, maxPersonnel);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Info.personnel == 0)
        {
            RoomManager.Instance.DestroyRoom(Info.uniqueId);
        }
    }

    public override void OnDestroy() => base.OnDestroy();

    public override void OnEnter(ClientSession session)
    {
        base.OnEnter(session);

        _sessions.TryAdd(session.SUID, session);

        // TODO: Broadcast
        // session.PlayerInfo
    }

    public override void OnLeave(ClientSession session)
    {
        base.OnLeave(session);

        _sessions.Remove(session.SUID);

        // TODO: Broadcast
        // session.PlayerInfo
    }

    public override void Broadcast(ClientSession session, IMessage message) => base.Broadcast(session, message);
}
