using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Server.Content.Data;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.Content.Room.WaitingRoom;

namespace Server.Content.Room;

public class GameRoom : RoomBase
{
    public GameRoomInfo Info { get { return (GameRoomInfo)_info; } }

    public override void OnStart(int uniqueId, int type, int maxPersonnel)
    {
        _info = new GameRoomInfo(uniqueId, type, maxPersonnel);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (_info.personnel == 0)
        {
            RoomManager.Instance.DestroyRoom(_info.uniqueId);
        }
    }

    public override void OnDestroy() => base.OnDestroy();

    public override void OnEnter(ClientSession session)
    {
        base.OnEnter(session);

        _sessions.TryAdd(session.SUID, session);
    }

    public override void OnLeave(ClientSession session)
    {
        base.OnLeave(session);

        _sessions.Remove(session.SUID);
    }

    public override void Broadcast(ClientSession session, IMessage message) => base.Broadcast(session, message);
}
