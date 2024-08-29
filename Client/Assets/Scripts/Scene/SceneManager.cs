using CoreLibrary.Job;
using CoreLibrary.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    SigninScene,
    LobbyScene,
    WaitingRoomScene,
    GameScene
}

public class SceneManager : MonoBehaviour
{
    public SceneBase CurrentScene { get; set; }

    public void LoadSceneAsync(SceneName sceneName, Action callback)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName, callback));
    }

    IEnumerator LoadSceneAsyncCoroutine(SceneName sceneName, Action callback)
    {
        var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);

        while (operation.isDone == false)
        {
            yield return null;
        }
        
        callback.Invoke();
    }
}
