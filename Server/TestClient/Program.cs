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

        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        Connector connector = new Connector();

        connector.Init(endPoint, args =>
        {
            Socket? connectSocket = args.ConnectSocket;
            SessionBase? session = SessionManager.Instance.Generate<GameSession>();
            if (connectSocket != null && session != null)
            {
                session.Init(connectSocket);
            }
        });

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
