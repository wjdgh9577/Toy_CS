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
    public class Connector
    {
        Socket _connectSocket;
        IPEndPoint _endPoint;

        public event Action<SocketAsyncEventArgs> Connected;

        public Connector(IPEndPoint endPoint)
        {
            _connectSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _endPoint = endPoint;
        }

        public void Start()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnCompleted);
            args.RemoteEndPoint = _endPoint;
            RegisterConnect(args);
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            try
            {
                bool pending = _connectSocket.ConnectAsync(args);
                if (pending == false)
                {
                    OnCompleted(null, args);
                }
            }
            catch (Exception ex)
            {
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
            }
        }

        void OnCompleted(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {
                    Connected?.Invoke(args);
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
        }
    }
}