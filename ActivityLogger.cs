using System;
using System.Collections.Generic;
using System.Linq;

public static class ActivityLogger
{
    private static List<string> logs = new List<string>();

    public static void Log(string message)
    {
        logs.Add(DateTime.Now + ": " + message);
    }

    public static List<string> GetRecent(int count = 5)
    {
        return logs.TakeLast(count).ToList();
    }
}
