using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Network
{
    /// <summary>
    /// Critical. 반드시 lock을 걸고 사용할 것.
    /// </summary>
    public class SendBuffer
    {
        public List<ArraySegment<byte>> BufferList { get; } = new List<ArraySegment<byte>>();

        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();

        object _lock = new object();

        public bool CheckPending()
        {
            BufferList.Clear();

            while (_sendQueue.Count > 0)
            {
                var d = _sendQueue.Dequeue();
                BufferList.Add(d);
            }

            return BufferList.Count > 0;
        }

        public void Add(ArraySegment<byte> data)
        {
            _sendQueue.Enqueue(data);
            if (BufferList.Count == 0)
            {
                while (_sendQueue.Count > 0)
                {
                    var d = _sendQueue.Dequeue();
                    BufferList.Add(d);
                }
            }
        }
    }
}