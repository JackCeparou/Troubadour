using System.Diagnostics;

namespace T4.Plugins.Troubadour;

public sealed class Throttler
{
    public long StartedAtMs { get; private set; }
    public int MsInterval { get; private set; }
    public long ElapsedMs => (long)Stopwatch.GetElapsedTime(StartedAtMs).TotalMilliseconds;

    public bool IsElapsed => ElapsedMs >= MsInterval;

    private Throttler()
    {
    }

    public void SetInterval(int ms) => MsInterval = ms;
    public void Reset() => StartedAtMs = Stopwatch.GetTimestamp();

    public void RunWhenElapsed(Action action)
    {
        if (!IsElapsed)
            return;

        action.Invoke();
        Reset();
    }

    public static Throttler Create(int ms)
    {
        return new Throttler { StartedAtMs = Stopwatch.GetTimestamp(), MsInterval = ms };
    }
}