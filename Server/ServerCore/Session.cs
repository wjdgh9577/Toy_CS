using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore;

public abstract class Session
{
    Socket _socket;
    RecvBuffer _recvBuffer;

    int _disconnected;

    public abstract void OnConnected();
    public abstract void OnDisconnected();
    public abstract void OnSend();
    public abstract void OnRecv(ArraySegment<byte> buffer);

    public void Init(Socket socket)
    {
        _socket = socket;
        _recvBuffer = new RecvBuffer(65535);

        _disconnected = 0;

        OnConnected();

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
        RegisterRecv(args);
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
    #endregion

    #region Recv

    void RegisterRecv(SocketAsyncEventArgs args)
    {
        if (_disconnected == 1)
            return;

        _recvBuffer.Clear();
        args.SetBuffer(_recvBuffer.WriteSegment);

        try
        {
            bool pending = _socket.ReceiveAsync(args);
            if (pending == false)
            {
                OnRecvCompleted(null, args);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            RegisterRecv(args);
        }
    }

    void OnRecvCompleted(object? sender, SocketAsyncEventArgs args)
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
            Console.WriteLine(ex);
        }
        finally
        {
            RegisterRecv(args);
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
