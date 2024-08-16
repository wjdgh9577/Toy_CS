using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Network;

public class NetworkHandler
{
    #region TCP/IP

    public static Listener TcpListener()
    {
        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        return new Listener(endPoint);
    }

    public static Listener TcpListener(string ipAddress, int port)
    {
        IPAddress address = IPAddress.Parse(ipAddress);
        IPEndPoint endPoint = new IPEndPoint(address, port);

        return new Listener(endPoint);
    }

    public static void TcpListener(out Listener listener)
    {
        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        listener = new Listener(endPoint);
    }

    public static void TcpListener(string ipAddress, int port, out Listener listener)
    {
        IPAddress address = IPAddress.Parse(ipAddress);
        IPEndPoint endPoint = new IPEndPoint(address, port);

        listener = new Listener(endPoint);
    }

    public static Connector TcpConnector()
    {
        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        return new Connector(endPoint);
    }

    public static Connector TcpConnector(string ipAddress, int port)
    {
        IPAddress address = IPAddress.Parse(ipAddress);
        IPEndPoint endPoint = new IPEndPoint(address, port);

        return new Connector(endPoint);
    }

    public static void TcpConnector(out Connector connector)
    {
        string host = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(host);
        IPAddress address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint endPoint = new IPEndPoint(address, 7777);

        connector = new Connector(endPoint);
    }

    public static void TcpConnector(string ipAddress, int port, out Connector connector)
    {
        IPAddress address = IPAddress.Parse(ipAddress);
        IPEndPoint endPoint = new IPEndPoint(address, port);

        connector = new Connector(endPoint);
    }

    #endregion
}
