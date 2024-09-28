using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
#if UNITY_EDITOR

    public const string COMMON_DATA_PATH = "../Common/Data";

#endif

    public const string MAP_PREFAB_PATH = "Prefabs/Map";
    public const string MAP_DATA_PATH = "Data/Map";
}
