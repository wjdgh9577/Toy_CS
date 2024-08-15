using CoreLibrary.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Room;

public struct RoomInfo : IEqualityComparer<RoomInfo>
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

    public bool Equals(RoomInfo x, RoomInfo y)
    {
        return x.uniqueId == y.uniqueId && x.id == y.id;
    }

    public int GetHashCode([DisallowNull] RoomInfo obj)
    {
        return obj.uniqueId.GetHashCode();
    }
}

public abstract class RoomBase
{
    public RoomInfo Info { get; protected set; }

    public virtual void OnStart(RoomInfo info)
    {
        Info = info;
    }

    public virtual void OnUpdate() { }

    public virtual void OnDestroy() { }

    public virtual void OnEnter(SessionBase session)
    {
        Info.Enter();
    }

    public virtual void OnLeave(SessionBase session)
    {
        Info.Leave();
    }
}
