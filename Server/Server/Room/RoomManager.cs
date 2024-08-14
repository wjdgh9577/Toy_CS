using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Room;

public class RoomManager
{
    public static RoomManager Instance { get; } = new RoomManager();
    RoomManager() { }

    const int MAX_CCU = 500;

    public int CCU { get; private set; } = 0;

    Dictionary<RoomInfo, RoomBase> _rooms = new Dictionary<RoomInfo, RoomBase>();

    int _newUniqueRoomId = 1;
    object _lock = new object();

    public void EnterRoom<T>(SessionBase session, int roomId) where T : RoomBase, new()
    {
        lock (_lock)
        {
            var room = FindRoom<T>(roomId);

            if (room == null)
                room = MakeRoom<T>(roomId);

            CCU += 1;
            room.OnEnter(session);
        }
    }

    public T MakeRoom<T>(int roomId) where T : RoomBase, new()
    {
        lock (_lock)
        {
            T newRoom = new T();
            RoomInfo info = new RoomInfo(_newUniqueRoomId++, roomId);
            _rooms.Add(info, newRoom);

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

            room?.OnDestroy();

            return true;
        }
    }
}
