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

namespace Server.Content.Room;

public class RoomManager
{
    public static RoomManager Instance { get; } = new RoomManager();
    RoomManager()
    {
        //JobTimerHandler.PushAfter(UpdateRooms, Define.ROOM_UPDATE_INTERVAL, autoReset: true);
    }

    Dictionary<int, RoomBase> _rooms = new Dictionary<int, RoomBase>();

    int _newUniqueRoomId = 1;
    object _lock = new object();

    public T? CreateRoom<T>(ClientSession session, int type, int maxPersonnel, Action<T> handleRoom = null) where T : RoomBase, new()
    {
        lock (_lock)
        {
            T room = MakeRoom<T>(type, maxPersonnel);

            handleRoom?.Invoke(room);

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

    public T? EnterRoom<T>(ClientSession session, int uniqueId, string password = null) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(uniqueId);

            if (room == null)
            {
                LogHandler.Log(LogCode.ROOM_NOT_EXIST, $"{typeof(T)}_{uniqueId} is not exist.");
                return null;
            }
            else if (room.ContainsSession(session.SUID))
            {
                LogHandler.LogError(LogCode.ROOM_SESSION_INVALID_UID, $"Session_({session.SUID}) is already entered.");
                return null;
            }
            else if (!room.Accessible)
            {
                LogHandler.Log(LogCode.CONSOLE, $"{typeof(T)}_{uniqueId} is already full.");
                return null;
            }
            else if (room is WaitingRoom waitingRoom && waitingRoom.Info.Verification(password) == false)
            {
                LogHandler.Log(LogCode.CONSOLE, $"Password fail: {password}");
                return null;
            }

            room.OnEnter(session);
            session.EnterRoom(room);

            return room;
        }
    }

    public T? LeaveRoom<T>(ClientSession session, int uniqueId) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(uniqueId);

            if (room == null)
            {
                LogHandler.LogError(LogCode.ROOM_NOT_EXIST, $"{typeof(T)}_{uniqueId} is not exist.");
                return null;
            }
            else if (room.ContainsSession(session.SUID) == false)
            {
                LogHandler.LogError(LogCode.ROOM_SESSION_NOT_EXIST, $"Session_{session.SUID} is not exist.");
                return null;
            }

            room.OnLeave(session);
            session.LeaveRoom(room);

            return room;
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

    T MakeRoom<T>(int type, int maxPersonnel) where T : RoomBase, new()
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
            _rooms.TryGetValue(uniqueId, out var room);

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

    public void Broadcast<T>(ClientSession session, int uniqueId, IMessage message) where T : RoomBase
    {
        lock (_lock)
        {
            var room = FindRoom<T>(uniqueId);

            if (room == null)
                return;

            room.Broadcast(session, message);
        }
    }

    /// <summary>
    /// 외부에서 Room에 접근하여 로직을 수행할 때 원자성 보장
    /// </summary>
    /// <param name="action"></param>
    public void Handle(Action action)
    {
        lock (_lock)
        {
            action?.Invoke();
        }
    }
}
