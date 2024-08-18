using CoreLibrary.Log;
using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SessionManager
{
    public static SessionManager Instance { get; } = new SessionManager();
    SessionManager() { }

    Dictionary<Type, SessionBase> _sessions = new Dictionary<Type, SessionBase>();

    object _lock = new object();

    public T Generate<T>() where T : SessionBase, new()
    {
        lock (_lock)
        {
            T session = new T();
                
            _sessions.TryAdd(typeof(T), session);

            return session;
        }
    }

    public T Find<T>() where T : SessionBase
    {
        lock (_lock)
        {
            if (_sessions.TryGetValue(typeof(T), out var session))
                return (T)session;

            return default;
        }
    }

    public void Remove<T>() where T : SessionBase
    {
        lock (_lock)
        {
            _sessions.Remove(typeof(T));
        }
    }
}