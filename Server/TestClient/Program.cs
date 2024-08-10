using CoreLibrary.Log;
using CoreLibrary.Network;
using System.Net;

namespace TestClient;

internal class Program
{
    static void Main(string[] args)
    {
        LogHandler.SetLogManager(new LogManager());

        Thread.Sleep(3000);

        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        Connector connector = new Connector();

        connector.Init(endPoint, () =>
        {
            return null;
        });

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
