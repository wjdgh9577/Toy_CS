using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get { Init();  return _instance; } }
    static Managers _instance;

    public NetworkManager NetworkManager { get; } = new NetworkManager();
    public AuthenticationManager AuthenticationManager { get; } = new AuthenticationManager();

    // MonoBehaviour
    public SceneManager SceneManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public ResourceManager ResourceManager { get; private set; }

    void Awake()
    {
        Init();
        
        SceneManager = gameObject.AddComponent<SceneManager>();
        UIManager = gameObject.AddComponent<UIManager>();
        GameManager = gameObject.AddComponent<GameManager>();
        ResourceManager = gameObject.AddComponent<ResourceManager>();
    }

    void Start()
    {
        NetworkManager.Start();
        AuthenticationManager.Start();
    }

    void Update()
    {
        NetworkManager.Update();
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        NetworkManager.Disconnect();
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleRight;

        GUI.Label(new Rect(Screen.width - 110, 10, 100, 20), $"Server Time: {NetworkManager.ServerTime}", style);
        GUI.Label(new Rect(Screen.width - 110, 40, 100, 20), $"Ping: {NetworkManager.ping}ms", style);
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
