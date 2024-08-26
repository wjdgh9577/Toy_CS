using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient.Session;

public class SessionManager
{
    public static SessionManager Instance { get; } = new SessionManager();
    SessionManager() { }

    public ServerSession Session { get; private set; }

    object _lock = new object();

    public ServerSession Generate()
    {
        lock (_lock)
        {
            Session = new ServerSession();

            return Session;
        }
    }
}
