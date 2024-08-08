using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore;

public class Listener
{
    Socket _listenSocket;
    Func<Session> _sessionFunc;

    public void Init(IPEndPoint endPoint, Func<Session> sessionFunc, int backlog = int.MaxValue)
    {
        _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _sessionFunc = sessionFunc;

        _listenSocket.Bind(endPoint);

        _listenSocket.Listen(backlog);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += new EventHandler<SocketAsyncEventArgs>(OnCompleted);
        RegisterAccept(args);
    }

    void RegisterAccept(SocketAsyncEventArgs args)
    {
        args.AcceptSocket = null;

        try
        {
            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
            {
                OnCompleted(null, args);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            RegisterAccept(args);
        }
    }

    void OnCompleted(object? sender, SocketAsyncEventArgs args)
    {
        try
        {
            if (args.SocketError == SocketError.Success)
            {
                Socket? acceptSocket = args.AcceptSocket;
                Session? session = _sessionFunc?.Invoke();
                if (acceptSocket != null && session != null)
                {
                    session.Init(acceptSocket);
                }
            }
            else
            {
                Console.WriteLine($"SocketError: {args.SocketError.ToString()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            RegisterAccept(args);
        }
    }
}
