using CoreLibrary.Job;
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

    Dictionary<int, RoomBase> _rooms = new Dictionary<int, RoomBase>();

    int _newUniqueRoomId = 1;
    object _lock = new object();

    public WaitingRoom CreateWaitingRoom(ClientSession session, int type, int maxPersonnel, string title, string password)
    {
        lock (_lock)
        {
            WaitingRoom room = MakeRoom<WaitingRoom>(type, maxPersonnel);
            room.Info.title = title;
            room.Info.password = password;

            room.OnEnter(session);
            session.EnterRoom(room);

            return room;
        }
    }

    public WaitingRoom? QuickWaitingRoom(ClientSession session)
    {
        lock (_lock)
        {
            var room = _rooms
                .Where(r => r.Value is WaitingRoom wr && string.IsNullOrEmpty(wr.Info.password) && wr.Info.personnel < wr.Info.maxPersonnel)
                .Select(r => r.Value)
                .FirstOrDefault() as WaitingRoom;

            if (room != null)
            {
                room.OnEnter(session);
                session.EnterRoom(room);
            }

            return room;
        }
    }

    public WaitingRoom EnterWaitingRoom(ClientSession session, int uniqueId, string password = null)
    {
        lock (_lock)
        {
            var room = FindRoom<WaitingRoom>(uniqueId);

            if (room == null)
            {
                LogHandler.Log(LogCode.ROOM_NOT_EXIST, $"WaitingRoom_{uniqueId} is not exist.");
                return null;
            }
            else if (room.ContainsSession(session.SUID))
            {
                LogHandler.LogError(LogCode.ROOM_SESSION_INVALID_UID, $"Session_({session.SUID}) is already entered.");
                return null;
            }
            else if (!room.Accessible)
            {
                LogHandler.Log(LogCode.CONSOLE, $"WaitingRoom_{uniqueId} is already full.");
                return null;
            }
            else if (room.Info.Verification(password) == false)
            {
                LogHandler.Log(LogCode.CONSOLE, $"Password fail: {password}");
                return null;
            }

            room.OnEnter(session);
            session.EnterRoom(room);

            return room;
        }
    }

    public bool LeaveRoom<T>(ClientSession session, int uniqueId) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(uniqueId);

            if (room == null)
            {
                LogHandler.LogError(LogCode.ROOM_NOT_EXIST, $"{typeof(T)}_{uniqueId} is not exist.");
                return false;
            }
            else if (room.ContainsSession(session.SUID) == false)
            {
                LogHandler.LogError(LogCode.ROOM_SESSION_NOT_EXIST, $"Session_{session.SUID} is not exist.");
                return false;
            }

            room.OnLeave(session);
            session.LeaveRoom(room);

            return true;
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

    public T MakeRoom<T>(int type, int maxPersonnel) where T : RoomBase, new()
    {
        lock (_lock)
        {
            int uniqueId = _newUniqueRoomId++;
            T newRoom = new T();

            _rooms.Add(uniqueId, newRoom);

            LogHandler.Log(LogCode.CONSOLE, $"Make room: Unique ID: {uniqueId}, ID: {type}");

            newRoom.OnStart(uniqueId, type, maxPersonnel);

            return newRoom;
        }
    }

    public T? FindRoom<T>(int uniqueId) where T : RoomBase
    {
        lock (_lock)
        {
            var room = _rooms
                .Where(r => r.Value is T && r.Key == uniqueId)
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
                .Where(r => r.Value is T && r.Value.ContainsSession(session.SUID))
                .Select(r => r.Value)
                .FirstOrDefault();

            return room as T;
        }
    }

    public List<T> GetRooms<T>() where T : RoomBase
    {
        lock (_lock)
        {
            List<T> rooms = new List<T>();

            foreach (var room in _rooms)
            {
                if (room.Value is T r)
                    rooms.Add(r);
            }

            return rooms;
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

    public bool DestroyRoom(int uniqueId)
    {
        lock (_lock)
        {
            if (_rooms.Remove(uniqueId, out var room) == false)
                return false;

            LogHandler.Log(LogCode.CONSOLE, $"Destroy room: {uniqueId}");

            room.OnDestroy();

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
