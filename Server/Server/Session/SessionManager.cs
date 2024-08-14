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

    Dictionary<int, List<SessionBase>> _sessions = new Dictionary<int, List<SessionBase>>();

    public int NewSUID => _suid++;
    int _suid = 0;

    object _lock = new object();

    public T Generate<T>(int suid) where T : SessionBase, new()
    {
        lock (_lock)
        {
            T session = new T();
            session.SUID = suid;

            if (_sessions.TryGetValue(suid, out var sessionList))
                sessionList.Add(session);
            else
                _sessions.Add(suid, new List<SessionBase>() { session });

            return session;
        }
    }

    public T? Find<T>(int suid) where T : SessionBase
    {
        lock ( _lock)
        {
            if (_sessions.TryGetValue(suid, out var sessionList))
            {
                T? session = sessionList.Find(s => s is T) as T;

                if (session != null)
                    return session;
            }

            return default;
        }
    }

    public void Remove<T>(T session) where T : SessionBase
    {
        lock (_lock)
        {
            int suid = session.SUID;

            if (_sessions.TryGetValue(suid, out var sessionList))
            {
                sessionList.Remove(session);

                if (sessionList.Count == 0)
                    _sessions.Remove(suid);
            }

        }
    }
}
