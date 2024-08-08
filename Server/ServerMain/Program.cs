using ServerCore;
using System.Net;

namespace ServerMain;

public class Program
{
    static void Main(string[] args)
    {
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
            Thread.Sleep(0);
        }
    }
}
