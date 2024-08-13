using CoreLibrary.Log;
using CoreLibrary.Network;
using Server.Session;
using System.Net;
using System.Net.Sockets;

namespace Server;

public class Program
{
    static void Main(string[] args)
    {
        LogHandler.SetModule(new LogModule());

        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        Listener listener = new Listener();

        listener.Init(endPoint, args =>
        {
            Socket? acceptSocket = args.AcceptSocket;
            int suid = SessionManager.Instance.NewSUID;
            SessionBase? session = SessionManager.Instance.Generate<GameSession>(suid);
            if (acceptSocket != null && session != null)
            {
                session.Init(acceptSocket);
            }
        });

        LogHandler.Log(LogCode.CONSOLE, "Listen...");

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
