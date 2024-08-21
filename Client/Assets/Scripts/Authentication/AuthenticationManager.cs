using CoreLibrary.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class AuthenticationManager : IManger
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

    public void Update()
    {
        
    }

    public void Authenticate(Action<bool> authenticated)
    {
        // TODO: 로그인 절차 구현
        authenticated.Invoke(true);
    }
}
