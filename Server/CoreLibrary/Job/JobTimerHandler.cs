using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Job;

public static class JobTimerHandler
{
    public static IJob PushAfter(IJob job, int interval, bool autoReset) { JobTimer.Instance.Push(job, interval, autoReset); return job; }
    public static IJob PushAfter(Action action, int interval, bool autoReset) { return PushAfter(new Job(action), interval, autoReset); }
    public static IJob PushAfter<T1>(Action<T1> action, T1 t1, int interval, bool autoReset) { return PushAfter(new Job<T1>(action, t1), interval, autoReset); }
    public static IJob PushAfter<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2, int interval, bool autoReset) { return PushAfter(new Job<T1, T2>(action, t1, t2), interval, autoReset); }
    public static IJob PushAfter<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3, int interval, bool autoReset) { return PushAfter(new Job<T1, T2, T3>(action, t1, t2, t3), interval, autoReset); }
}
