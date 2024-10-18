using CoreLibrary.Log;
using CoreLibrary.Network;
using CoreLibrary.Utility;
using Server.Content.Info;
using Server.Data;
using Server.Data.Map;
using Server.Session;
using Server.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        // 플레이어 위치 초기화 필요시 여기서
        //Info.players

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
        var position = new CustomVector2(info.Transform.XPos, info.Transform.YPos);
        var colider = new ColliderInfo()
            .SetOffset(new CustomVector2(position.x + info.Collider.XOffset, position.y + info.Collider.YOffset))
            .SetWidth(info.Collider.Width)
            .SetHeight(info.Collider.Height);
        var rectVertices = colider.RectVertices;
        var mapColliderPaths = map.colliderPaths;

        // 독립된 지형별 검증
        foreach (var mapColliderPath in mapColliderPaths)
        {
            var mapColliderPathCount = mapColliderPath.Count;

            // rectangle - ray casting
            foreach (var vertex in rectVertices)
            {
                var crossCount = 0;

                for (int i = 0; i < mapColliderPathCount; i++)
                {
                    var current = mapColliderPath[i];
                    var next = mapColliderPath[(i + 1) % mapColliderPathCount];

                    if (vertex.y.IsBetween(current.y, next.y) && GetX(current, next, vertex.y) > vertex.x)
                        crossCount++;
                }

                if (crossCount % 2 == 1)
                    return false;
            }

            // half circle - radius
            var radius = colider.Radius;
            var lower = colider.LowerCenter;
            var upper = colider.UpperCenter;
            
            for (int i = 0; i < mapColliderPathCount; i++)
            {
                var current = mapColliderPath[i];
                var next = mapColliderPath[(i + 1) % mapColliderPathCount];
                var currentToNext = (CustomVector2)(next - current);

                // lower
                if (CheckCircleCollision(current, next, lower, radius))
                    return false;

                // upper
                if (CheckCircleCollision(current, next, upper, radius))
                    return false;
            }
        }

        return true;

        float GetX(CustomVector2 a, CustomVector2 b, float y)
        {
            return (y - a.y) * (b.x - a.x) / (b.y - a.y) + a.x;
        }

        bool CheckCircleCollision(CustomVector2 a, CustomVector2 b, CustomVector2 center, float radius)
        {
            var ab = b - a;
            var dot = ab.Dot(center - a);
            var toCurrentSquare = (a - center).Square();
            var toNextSquare = (b - center).Square();

            var distanceSquare = 0f;
            // 수선이 두 점 사이인 경우
            if (dot > 0 && dot <= ab.Square())
            {
                distanceSquare = DistanceSquare(a, b, center);
            }
            // 수선이 두 점 바깥인 경우
            else
            {
                distanceSquare = Math.Min(toCurrentSquare, toNextSquare);
            }

            if (distanceSquare < radius * radius)
                return true;

            return false;
        }

        float DistanceSquare(CustomVector2 a, CustomVector2 b, CustomVector2 t)
        {
            float _a = a.y - b.y;
            float _b = b.x - a.x;
            float _c = a.x * b.y - a.y * b.x;
            
            return (_a * t.x + _b * t.y + _c) * ((_a * t.x + _b * t.y + _c) / (_a * _a + _b * _b));
        }
    }
}
