using CoreLibrary.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Map Info")]
public class MapInfo : ScriptableObject, IInfo<int>
{
    public int uniqueId;
    public string mapName;
    public string prefabName;

    public int GetKey() => uniqueId;
}
