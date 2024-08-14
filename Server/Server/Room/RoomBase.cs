using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Room;

public struct RoomInfo
{
    /// <summary>
    /// Room의 고유식별자
    /// </summary>
    public int uniqueId;

    /// <summary>
    /// Room의 종류에 따른 식별자
    /// </summary>
    public int id;

    /// <summary>
    /// 현재 접속자 수
    /// </summary>
    public int ccu;

    public RoomInfo(int uniqueId, int id)
    {
        this.uniqueId = uniqueId;
        this.id = id;
        this.ccu = 0;
    }

    public void Enter()
    {
        this.ccu += 1;
    }

    public void Leave()
    {
        this.ccu = Math.Max(this.ccu - 1, 0);
    }
}

public abstract class RoomBase
{
    public RoomInfo Info { get; protected set; }

    public virtual void OnStart(RoomInfo info)
    {
        Info = info;
    }

    public virtual void OnUpdate()
    {
        if (_destroyed == 0)
            return;
    }

    public virtual void OnDestroy() { }

    public virtual void OnEnter(SessionBase session)
    {
        if (_destroyed == 0)
            return;

        Info.Enter();
    }

    public virtual void OnLeave(SessionBase session)
    {
        if (_destroyed == 0)
            return;

        Info.Leave();
    }
}
