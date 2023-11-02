using System;

namespace ECommons.Schedulers;

public class TickScheduler : IScheduler
{
    long executeAt;
    Action function;
    bool disposed = false;

    public TickScheduler(Action function, long delayMS = 0)
    {
        executeAt = Environment.TickCount64 + delayMS;
        this.function = function;
        Services.Framework.Update += Execute;
    }

    public void Dispose()
    {
        if (!disposed)
        {
            Services.Framework.Update -= Execute;
        }
        disposed = true;
    }

    void Execute(object _)
    {
        if (Environment.TickCount64 < executeAt) return;
        try
        {
            function();
        }
        catch (Exception e)
        {
            Services.PluginLog.Error(e.Message + "\n" + e.StackTrace ?? "");
        }
        Dispose();
    }
}
