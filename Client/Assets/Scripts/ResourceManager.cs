using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Dictionary<int, MapInfo> MapInfos => _mapInfos;
    Dictionary<int, MapInfo> _mapInfos;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        LoadData<int, MapInfo>(Config.MAP_DATA_PATH, out _mapInfos);
    }

    void LoadData<TKey, TValue>(string path, out Dictionary<TKey, TValue> infos) where TValue : UnityEngine.Object, IInfo<TKey>
    {
        infos = new Dictionary<TKey, TValue>();

        TValue[] resources = Resources.LoadAll<TValue>(path);

        foreach (TValue resource in resources)
        {
            infos.Add(resource.GetKey(), resource);
        }
    }

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        T obj = Resources.Load<T>(path);

        return obj;
    }

    public T Instantiate<T>(string path, string name) where T : UnityEngine.Object
    {
        T obj = Load<T>($"{path}/{name}");

        return Instantiate(obj);
    }
}
