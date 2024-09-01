using CoreLibrary.Log;
using CoreLibrary.Network;
using System.Net;
using System.Net.Sockets;
using TestClient.Session;
using TestClient.Log;

namespace TestClient;

public class Program
{
    const int TEST_CLIENT_COUNT = 1;

    static void Main(string[] args)
    {
        LogHandler.SetModule(new LogModule());
        
        Thread.Sleep(3000);

        for (int i = 0; i < TEST_CLIENT_COUNT; i++)
        {
            NetworkHandler.TcpConnector(out var connector);
            connector.Connected += (connected, args) =>
            {
                if (connected)
                {
                    Socket? connectSocket = args.ConnectSocket;
                    SessionBase? session = SessionManager.Instance.Generate();
                    if (connectSocket != null && session != null)
                    {
                        session.Start(connectSocket);
                    }
                }
                else
                {
                    connector.Start();
                    Thread.Sleep(10);
                }
            };

            connector.Start();
            Thread.Sleep(10);
        }

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
