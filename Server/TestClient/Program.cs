using CoreLibrary.Log;
using CoreLibrary.Network;
using System.Net;
using System.Net.Sockets;
using TestClient.Session;

namespace TestClient;

internal class Program
{
    static void Main(string[] args)
    {
        LogHandler.SetModule(new LogModule());
        
        Thread.Sleep(3000);

        NetworkHandler.TcpConnector(out var connector);
        connector.Connected += args =>
        {
            Socket? connectSocket = args.ConnectSocket;
            SessionBase? session = SessionManager.Instance.Generate<GameSession>();
            if (connectSocket != null && session != null)
            {
                session.Start(connectSocket);
            }
        };

        connector.Start();

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
