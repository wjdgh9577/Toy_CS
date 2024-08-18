using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreLibrary.Log;

namespace CoreLibrary.Network
{
    public abstract class SessionBase
    {
        protected Socket _socket;

        protected SendBuffer _sendBuffer;
        protected RecvBuffer _recvBuffer;

        protected SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        protected SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        protected int _disconnected = 0;

        object _lock = new object();

        public int SUID { get; set; }

        public abstract void OnConnected();
        public abstract void OnDisconnected();
        public abstract void OnSend(int BytesTransferred);
        public abstract void OnRecv(ArraySegment<byte> buffer);

        public void Start(Socket socket, int recvBufferSize = 65535)
        {
            _socket = socket;

            _sendBuffer = new SendBuffer();
            _recvBuffer = new RecvBuffer(recvBufferSize);

            OnConnected();

            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendComplete);
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            RegisterRecv();
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            OnDisconnected();

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        #region Send

        protected void Send(ArraySegment<byte> data)
        {
            lock (_lock)
            {
                _sendBuffer.Add(data);

                if (_sendBuffer.BufferList.Count > 0)
                {
                    RegisterSend();
                }
            }
        }

        protected void RegisterSend()
        {
            if (_disconnected == 1)
                return;

            _sendArgs.BufferList = _sendBuffer.BufferList;

            try
            {
                bool pending = _socket.SendAsync(_sendArgs);
                if (pending == false)
                {
                    OnSendComplete(null, _sendArgs);
                }
            }
            catch (Exception ex)
            {
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
            }
        }

        protected void OnSendComplete(object sender, SocketAsyncEventArgs args)
        {
            if (_disconnected == 1)
                return;

            if (args.BytesTransferred == 0 || args.SocketError != SocketError.Success)
            {
                Disconnect();
                return;
            }

            try
            {
                lock (_lock)
                {
                    _sendArgs.BufferList = null;

                    OnSend(args.BytesTransferred);

                    bool pending = _sendBuffer.CheckPending();
                    if (pending)
                        RegisterSend();
                }
            }
            catch (Exception ex)
            {
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
            }
        }

        #endregion

        #region Recv

        protected void RegisterRecv()
        {
            if (_disconnected == 1)
                return;

            _recvBuffer.Clear();

            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs);
                if (pending == false)
                {
                    OnRecvCompleted(null, _recvArgs);
                }
            }
            catch (Exception ex)
            {
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
                RegisterRecv();
            }
        }

        protected void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (_disconnected == 1)
                return;

            if (args.BytesTransferred == 0 || args.SocketError != SocketError.Success)
            {
                Disconnect();
                return;
            }

            try
            {
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                {
                    Disconnect();
                    return;
                }

                ReadBuffer();
            }
            catch (Exception ex)
            {
                LogHandler.LogError(LogCode.EXCEPTION, ex.ToString());
            }
            finally
            {
                RegisterRecv();
            }
        }

        void ReadBuffer()
        {
            ArraySegment<byte> segment = _recvBuffer.ReadSegment;

            while (true)
            {
                if (segment.Count < sizeof(ushort))
                    break;

                ushort dataSize = BitConverter.ToUInt16(segment.Array, segment.Offset);
                if (segment.Count < dataSize)
                    break;

                if (_recvBuffer.OnRead(dataSize) == false)
                    break;

                OnRecv(new ArraySegment<byte>(segment.Array, segment.Offset, dataSize));

                segment = new ArraySegment<byte>(segment.Array, segment.Offset + dataSize, segment.Count - dataSize);
            }
        }

        #endregion
    }
}