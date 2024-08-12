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

    Dictionary<int, SessionBase> _sessions = new Dictionary<int, SessionBase>();

    int _sessionIdGen = 0;

    object _lock = new object();

    public T Generate<T>() where T : SessionBase, new()
    {
        lock (_lock)
        {
            int sessionId = _sessionIdGen++;

            T session = new T();
            session.SessionId = sessionId;
            _sessions.Add(sessionId, session);

            return session as T;
        }
    }

    public void Remove<T>(T session) where T : SessionBase
    {
        lock (_lock)
        {
            _sessions.Remove(session.SessionId);
        }
    }
}
