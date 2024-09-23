using CoreLibrary.Log;
using Newtonsoft.Json;
using Server.Data.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data;

interface IData<T>
{
    T GetKey();
}

public class DataManager
{
    class SerializedData<T>
    {
        public List<T> datas = new List<T>();
    }

    #region Singleton

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DataManager();
                }
            }

            return _instance;
        }
    }
    static DataManager _instance;
    static object _lock = new object();

    DataManager() { }

    #endregion

    // Data Dictionary
    public Dictionary<int, MapData> MapDataDic { get; private set; }

    public void Init()
    {
        Deserialize<int, MapData>(MapDataDic);
    }

    void Deserialize<TKey, TValue>(Dictionary<TKey, TValue> dic) where TValue : IData<TKey>
    {
        dic = new Dictionary<TKey, TValue>();
        string json = LoadJsonData(typeof(TValue).Name);

        if (string.IsNullOrEmpty(json))
        {
            LogHandler.LogError(LogCode.CONSOLE, "Missing json data.", typeof(TValue).Name);
            return;
        }

        var obj = JsonConvert.DeserializeObject<SerializedData<TValue>>(json);
        foreach (var data in obj.datas)
        {
            if (dic.TryAdd(data.GetKey(), data) == false)
            {
                LogHandler.LogError(LogCode.CONSOLE, "Duplicated key.", data.GetKey());
            }
        }
    }

    string LoadJsonData(string path)
    {
        path = Path.GetFullPath($"../../../Data/JsonData/{path}.json");
        string json = "";

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }

        return json;
    }
}
