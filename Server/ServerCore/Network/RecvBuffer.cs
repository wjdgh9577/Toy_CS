using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Network;

public class RecvBuffer
{
    ArraySegment<byte> _buffer;
    int _readOffset;
    int _writeOffset;

    public RecvBuffer() : this(65535) { }

    public RecvBuffer(int bufferSize)
    {
        _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        _readOffset = _writeOffset = 0;
    }

    public int DataSize => _writeOffset - _readOffset;
    public int FreeSize => _buffer.Count - _writeOffset;

    public ArraySegment<byte> ReadSegment => new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readOffset, DataSize);
    public ArraySegment<byte> WriteSegment => new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writeOffset, FreeSize);

    public void Clear()
    {
        int dataSize = DataSize;

        if (dataSize == 0)
        {
            _readOffset = _writeOffset = 0;
        }
        else
        {
            Array.Copy(_buffer.Array, _buffer.Offset + _readOffset, _buffer.Array, _buffer.Offset, dataSize);
            _readOffset = 0;
            _writeOffset = dataSize;
        }
    }

    public bool OnRead(int bytes)
    {
        if (bytes > DataSize)
            return false;

        _readOffset += bytes;

        return true;
    }

    public bool OnWrite(int bytes)
    {
        if (bytes > FreeSize)
            return false;

        _writeOffset += bytes;

        return true;
    }
}
