using CoreLibrary.Job;
using CoreLibrary.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class AuthenticationManager
{
    #region Singleton

    int _instantiated = 0;
    public AuthenticationManager()
    {
        if (Interlocked.Exchange(ref _instantiated, 1) == 1)
        {
            LogHandler.LogError(LogCode.EXCEPTION, $"{this} is already instantiated.");
            throw new Exception();
        }
    }

    #endregion

    string guid;

    public void Start()
    {
        // Test
        guid = Guid.NewGuid().ToString();
    }

    public void Authenticate(string id, string pw, Action<bool> authenticateCallback)
    {
        // TODO: �α��� ���� ����
        LogHandler.Log(LogCode.CONSOLE, $"Authenticated: {id}");
        authenticateCallback.Invoke(true);
    }
}
