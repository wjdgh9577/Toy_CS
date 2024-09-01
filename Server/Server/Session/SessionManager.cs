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

    Dictionary<int, ClientSession> _clientSessions = new Dictionary<int, ClientSession>();
    // 서버간 통신 전용
    //Dictionary<int, ServerSession> _serverSessions = new Dictionary<int, ServerSession>();

    public int CCU { get; private set; } = 0;

    int NewSUID => _suid++;
    int _suid = 0;

    object _lock = new object();

    public ClientSession Generate()
    {
        lock (_lock)
        {
            ClientSession session = new ClientSession();
            
            int suid = session.SUID = NewSUID;

            // TODO: 토큰 발급
            session.Token = Guid.NewGuid().ToString(); // 테스트

            if (_clientSessions.TryAdd(suid, session) == false)
                LogHandler.LogError(LogCode.SESSION_INVALID_UID, $"SUID ({suid}) is already used.");

            CCU += 1;

            return session;
        }
    }

    public ClientSession? Find(int suid)
    {
        lock ( _lock)
        {
            if (_clientSessions.TryGetValue(suid, out var session))
            {
                return session;
            }

            return default;
        }
    }

    public void Remove(ClientSession session)
    {
        lock (_lock)
        {
            if (_clientSessions.Remove(session.SUID) == false)
                LogHandler.LogError(LogCode.SESSION_NOT_EXIST, $"Session_{session.SUID} is not exist.");

            CCU = Math.Max(CCU - 1, 0);
        }
    }
}
