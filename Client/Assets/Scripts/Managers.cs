using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManger
{
    void Start();
    void Update();
}

public class Managers : MonoBehaviour
{
    public static Managers Instance { get { Init();  return _instance; } }
    static Managers _instance;

    public NetworkManager NetworkManager { get; } = new NetworkManager();
    public AuthenticationManager AuthenticationManager { get; } = new AuthenticationManager();

    void Awake()
    {
        Init();
    }

    void Start()
    {
        NetworkManager.Start();
        AuthenticationManager.Start();
    }

    void Update()
    {
        NetworkManager.Update();
        AuthenticationManager.Update();
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        NetworkManager.Disconnect();
    }
#endif

    static void Init()
    {
        if (_instance == null)
        {
            GameObject managerObject = GameObject.Find("Managers");
            if (managerObject == null)
            {
                managerObject = new GameObject("Managers");
                _instance = managerObject.AddComponent<Managers>();
            }

            DontDestroyOnLoad(managerObject);
        }
    }
}
