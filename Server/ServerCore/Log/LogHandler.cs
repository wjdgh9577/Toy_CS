using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Log;

public static class LogHandler
{
    static LogBase? Instance;

    public static void SetLogManager(LogBase manager) => Instance = manager;

    public static void Log(LogCode code, params object[] args) => Instance?.Log(LogType.NONE, code, args);
    public static void Log(LogCode code, params (object, object)[] args) => Instance?.Log(LogType.NONE, code, args);
    public static void LogWarning(LogCode code, params object[] args) => Instance?.Log(LogType.WARNING, code, args);
    public static void LogWarning(LogCode code, params (object, object)[] args) => Instance?.Log(LogType.WARNING, code, args);
    public static void LogError(LogCode code, params object[] args) => Instance?.Log(LogType.ERROR, code, args);
    public static void LogError(LogCode code, params (object, object)[] args) => Instance?.Log(LogType.ERROR, code, args);
}

public abstract class LogBase
{
    public abstract void Log(LogType type, LogCode code, params object[] args);
    public abstract void Log(LogType type, LogCode code, params (object key, object value)[] args);

    protected (string importance, string code) GetCommonMessage(LogType type, LogCode code)
    {
        string importanceMessage = "";
        switch (type)
        {
            case LogType.NONE:
                importanceMessage = "Level: 1";
                break;
            case LogType.WARNING:
                importanceMessage = "Level: 2";
                break;
            case LogType.ERROR:
                importanceMessage = "Level: 3";
                break;
        }

        return (importanceMessage, code.ToString());
    }
}
