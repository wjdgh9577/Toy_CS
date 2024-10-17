using CoreLibrary.Log;
using CoreLibrary.Network;
using CoreLibrary.Utility;
using Server.Content.Info;
using Server.Data;
using Server.Data.Map;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Room;

public class GameRoom : RoomBase
{
    public GameRoomInfo Info { get { return (GameRoomInfo)_info; } }
    private MapData map;

    public override void OnStart(int uniqueId, int type, int maxPersonnel)
    {
        _info = new GameRoomInfo(uniqueId, type, maxPersonnel);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        GameLogic();

        RefreshGameRoom();
    }

    public override void OnDestroy() => base.OnDestroy();

    public override void OnEnter(ClientSession session) => base.OnEnter(session);

    public override void OnLeave(ClientSession session) => base.OnLeave(session);

    public void SetGame(int mapId)
    {
        Info.mapId = mapId;

        if (!DataManager.Instance.MapDataDic.TryGetValue(mapId, out map) || map == null)
        {
            LogHandler.LogError(LogCode.GAME_INVALID_MAPID, $"Invalid map id: {mapId}");
            return;
        }

        Broadcast(PacketHandler.S_EnterGameRoom(Info.GetProto()));
    }

    public void WaitGame(ClientSession session)
    {
        Info.ReadyPlayer(session.AccountInfo.Uuid);

        if (Info.IsReady)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Info.StartGame();

        Broadcast(PacketHandler.S_StartGame());
    }

    public void GameLogic()
    {
        
    }

    public void RefreshGameRoom()
    {
        if (Info.IsDirty)
        {
            Broadcast(PacketHandler.S_SyncPlayer(Info.GetProto().Players));
        }
    }

    public void UpdatePlayerInfo(Google.Protobuf.Protocol.GameRoomPlayerInfo info)
    {
        var player = Info.players[info.BaseInfo.AccountInfo.Uuid];

        if (VerifyPosition(info))
            player.SetProto(info);
        else
        {
            var proto = player.GetProto();
            proto.IsValid = false;
            _sessions.Values
                .First(s => s.AccountInfo.Uuid == info.BaseInfo.AccountInfo.Uuid)
                .Send(PacketHandler.S_SyncPlayer(new Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.GameRoomPlayerInfo>() { proto }));
        }
    }

    bool VerifyPosition(Google.Protobuf.Protocol.GameRoomPlayerInfo info)
    {
        bool isValid = true;

        // TODO: 플레이어 위치 무결성 검증
        var position = new CustomVector2(info.Transform.XPos, info.Transform.YPos);
        // var colider = info.
        var colliderPaths = map.colliderPaths;

        return isValid;
    }
}
