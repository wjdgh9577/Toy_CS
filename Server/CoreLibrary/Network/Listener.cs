using CoreLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Network
{
    public class Listener
    {
        Socket _listenSocket;
        IPEndPoint _endPoint;

        public event Action<SocketAsyncEventArgs> Accepted;

        public Listener(IPEndPoint endPoint)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _endPoint = endPoint;
        }

        public void Start(int backlog = int.MaxValue)
        {
            _listenSocket.Bind(_endPoint);

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
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
                RegisterAccept(args);
            }
        }

        void OnCompleted(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {
                    Accepted?.Invoke(args);
                }
                else
                {
                    LogHandler.LogError(LogCode.SOCKET_ERROR, args.SocketError.ToString());
                }
            }
            catch (Exception ex)
            {
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
            }
            finally
            {
                RegisterAccept(args);
            }
        }
    }
}