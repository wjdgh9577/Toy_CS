using CoreLibrary.Network;
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

    public override void OnEnter(SessionBase session)
    {
        base.OnEnter(session);
    }
}
