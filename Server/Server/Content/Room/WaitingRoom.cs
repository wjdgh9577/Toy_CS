using CoreLibrary.Log;
using Google.Protobuf;
using Server.Content.Info;
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

    public override void OnUpdate() => base.OnUpdate();

    public override void OnDestroy() => base.OnDestroy();

    public override void OnEnter(ClientSession session)
    {
        base.OnEnter(session);

        Broadcast(session, PacketHandler.S_RefreshWaitingRoom(Info.GetProto()));
    }

    public override void OnLeave(ClientSession session)
    {
        base.OnLeave(session);

        Broadcast(session, PacketHandler.S_RefreshWaitingRoom(Info.GetProto()));
    }

    public void StartGame()
    {
        var playerSessions = SessionManager.Instance.ClientSessions
            .Join(Info.players, x => x.Value.AccountInfo.Uuid, y => y.Key, (x, y) => x.Value)
            .ToList();

        var host = playerSessions[0];
        var gameRoom = RoomManager.Instance.CreateRoom<GameRoom>(host, Info.type, Info.maxPersonnel);
        for (int i = 1; i < playerSessions.Count; i++)
        {
            gameRoom = RoomManager.Instance.EnterRoom<GameRoom>(playerSessions[i], gameRoom.Info.uniqueId);
        }

        gameRoom.SetGame(Info.mapId);
    }
}
