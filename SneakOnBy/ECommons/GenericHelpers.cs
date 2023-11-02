using System;

namespace ECommons;

public static unsafe class GenericHelpers
{
    public static void Safe(System.Action a, Action<string> fail, bool suppressErrors = false)
    {
        try
        {
            a();
        }
        catch (Exception e)
        {
            try
            {
                fail(e.Message);
            }
            catch(Exception ex)
            {
                Services.PluginLog.Error("Error while trying to process error handler:");
                Services.PluginLog.Error($"{ex.Message}\n{ex.StackTrace ?? ""}");
                suppressErrors = false;
            }
            if (!suppressErrors) Services.PluginLog.Error($"{e.Message}\n{e.StackTrace ?? ""}");
        }
    }
}
