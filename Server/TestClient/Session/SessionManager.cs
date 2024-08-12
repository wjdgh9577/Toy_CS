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

    public T Generate<T>() where T : SessionBase, new()
    {
        T session = new T();

        return session as T;
    }
}
