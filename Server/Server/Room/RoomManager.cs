﻿using CoreLibrary.Job;
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

public class RoomManager
{
    public static RoomManager Instance { get; } = new RoomManager();
    RoomManager()
    {
        JobTimerHandler.PushAfter(UpdateRooms, Define.ROOM_UPDATE_INTERVAL, autoReset: true);
    }

    public int CCU { get; private set; } = 0;

    Dictionary<RoomInfo, RoomBase> _rooms = new Dictionary<RoomInfo, RoomBase>();

    int _newUniqueRoomId = 1;
    object _lock = new object();

    public void EnterRoom<T>(ClientSession session, int roomId) where T : RoomBase, new()
    {
        lock (_lock)
        {
            var room = FindRoom<T>(roomId);

            if (room == null)
                room = MakeRoom<T>(roomId);

            CCU += 1;
            room.OnEnter(session);
            session.EnterRoom(room);
        }
    }

    public void LeaveRoom<T>(ClientSession session, int roomId) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(roomId);

            if (room == null)
            {
                LogHandler.LogError(LogCode.ROOM_NOT_EXIST, $"{typeof(T)}_{roomId} is not exist.");
                return;
            }

            CCU = Math.Max(CCU - 1, 0);
            room.OnLeave(session);
            session.LeaveRoom(room);
        }
    }

    public void LeaveRoom(ClientSession session, HashSet<RoomBase> roomList)
    {
        lock (_lock)
        {
            foreach (var room in roomList)
            {
                room.OnLeave(session);
                session.LeaveRoom(room);
            }
        }
    }

    public T MakeRoom<T>(int roomId) where T : RoomBase, new()
    {
        lock (_lock)
        {
            T newRoom = new T();
            RoomInfo info = new RoomInfo(_newUniqueRoomId++, roomId);
            _rooms.Add(info, newRoom);

            LogHandler.Log(LogCode.CONSOLE, $"Make room: {info.ToString()}");

            newRoom.OnStart(info);

            return newRoom;
        }
    }

    public T? FindRoom<T>(int roomId) where T : RoomBase
    {
        lock (_lock)
        {
            var room = _rooms
                .Where(r => r.Value is T && r.Key.id == roomId)
                .Select(r => r.Value)
                .FirstOrDefault();

            return room as T;
        }
    }

    public T? FindRoom<T>(ClientSession session) where T : RoomBase
    {
        lock (_lock)
        {
            var room = _rooms
                .Where(r => r.Value.GetType() == typeof(T) && r.Value.TryGetSession(session.SUID, out var s))
                .Select(r => r.Value)
                .FirstOrDefault();

            return room as T;
        }
    }

    public void UpdateRooms()
    {
        lock (_lock)
        {
            foreach (var room in _rooms.Values)
            {
                room.OnUpdate();
            }
        }
    }

    public bool DestroyRoom<T>(int roomId) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(roomId);

            if (room == null)
                return false;

            var info = room.Info;
            _rooms.Remove(info);

            LogHandler.Log(LogCode.CONSOLE, $"Destroy room: {info.ToString()}");

            room?.OnDestroy();

            return true;
        }
    }

    public void Broadcast<T>(ClientSession session, IMessage message) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(session);

            if (room == null)
                return;

            room.Broadcast(session, message);
        }
    }
}
