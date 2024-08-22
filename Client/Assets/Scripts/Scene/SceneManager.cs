using CoreLibrary.Job;
using CoreLibrary.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName, Action callback)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName, callback));
    }

    IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action callback)
    {
        var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        
        while (operation.isDone == false)
        {
            yield return null;
        }

        callback.Invoke();
    }
}
