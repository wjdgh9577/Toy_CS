using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CoreLibrary.Job;

public class JobTimer
{
    public static JobTimer Instance = new JobTimer();
    JobTimer() { }

    JobTimerPool _pool = new JobTimerPool();

    public void Push(IJob job, int interval, bool autoReset)
    {
        var element = _pool.Get();

        element.SetTimer((s, e) =>
        {
            job.Excute();
            _pool.Release(element);
        }, interval, autoReset);

        element.Start();
    }
}

struct JobTimerElement
{
    System.Timers.Timer _timer;

    ElapsedEventHandler? _handler;

    public JobTimerElement()
    {
        _timer = new System.Timers.Timer();
        _handler = null;
        Reset();
    }

    public void Reset()
    {
        _timer.Enabled = false;
        _timer.Elapsed -= _handler;
    }

    public void SetTimer(ElapsedEventHandler handler, int interval, bool autoReset)
    {
        _handler = handler;
        _timer.Interval = interval;
        _timer.AutoReset = autoReset;
    }

    public void Start()
    {
        _timer.Elapsed += _handler;
        _timer.Enabled = true;
        _timer.Start();
    }
}

class JobTimerPool
{
    Queue<JobTimerElement> _timers = new Queue<JobTimerElement>();
    object _lock = new object();

    public JobTimerElement Get()
    {
        JobTimerElement timer;

        lock (_lock)
        {
            if (_timers.Count == 0)
                timer = new JobTimerElement();
            else
                timer = _timers.Dequeue();
        }

        return timer;
    }

    public void Release(JobTimerElement element)
    {
        lock (_lock)
        {
            element.Reset();
            _timers.Enqueue(element);
        }
    }
}