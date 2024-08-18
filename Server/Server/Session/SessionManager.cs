using CoreLibrary.Log;
using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session;

public class SessionManager
{
    public static SessionManager Instance { get; } = new SessionManager();
    SessionManager() { }

    Dictionary<int, SessionBase> _sessions = new Dictionary<int, SessionBase>();

    int NewSUID => _suid++;
    int _suid = 0;

    object _lock = new object();

    public T Generate<T>() where T : SessionBase, new()
    {
        lock (_lock)
        {
            T session = new T();
            int suid = session.SUID = NewSUID;

            if (_sessions.TryAdd(suid, session) == false)
                LogHandler.LogError(LogCode.SESSION_INVALID_UID, $"SUID ({suid}) is already used.");

            return session;
        }
    }

    public T? Find<T>(int suid) where T : SessionBase
    {
        lock ( _lock)
        {
            if (_sessions.TryGetValue(suid, out var session))
            {
                return session as T;
            }

            return default;
        }
    }

    public void Remove<T>(T session) where T : SessionBase
    {
        lock (_lock)
        {
            if (_sessions.Remove(session.SUID) == false)
                LogHandler.LogError(LogCode.SESSION_NOT_EXIST, $"Session_{session.SUID} is not exist.");
        }
    }
}
