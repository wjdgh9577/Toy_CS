﻿using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
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
        // TODO: 플레이어 위치 무결성 검증
    }

    public void RefreshGameRoom()
    {
        if (Info.IsDirty)
        {
            // TODO: 동기화 패킷 전송
            //Broadcast();
        }
    }
}
