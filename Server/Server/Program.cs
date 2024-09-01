using CoreLibrary.Log;
using CoreLibrary.Network;
using Server.Session;
using Server.Log;
using System.Net;
using System.Net.Sockets;

namespace Server;

public class Program
{
    static void Main(string[] args)
    {
        LogHandler.SetModule(new LogModule());

        NetworkHandler.TcpListener(out var listener);
        listener.Accepted += args =>
        {
            Socket? acceptSocket = args.AcceptSocket;
            ClientSession? session = SessionManager.Instance.Generate();
            if (acceptSocket != null && session != null)
            {
                session.Start(acceptSocket);
            }
        };
        
        listener.Start();

        LogHandler.Log(LogCode.CONSOLE, "Listen...");

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
