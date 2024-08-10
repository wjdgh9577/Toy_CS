using CoreLibrary.Log;
using CoreLibrary.Network;
using System.Net;

namespace Server;

public class Program
{
    static void Main(string[] args)
    {
        LogHandler.SetLogManager(new LogManager());

        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        Listener listener = new Listener();
        //listener.Init(endPoint, () =>
        //{
        //    return new Session();
        //});

        while (true)
        {
            LogHandler.Log(LogCode.CONSOLE, "Listen...");
            Thread.Sleep(1000);
        }
    }
}
