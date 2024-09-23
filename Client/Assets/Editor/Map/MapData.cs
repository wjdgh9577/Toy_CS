using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreLibrary.Utility;

#if UNITY_EDITOR
using UnityEditor;

public class MapData : IData
{
    public int uniqueId;
    public List<List<CustomVector2Int>> colliderPaths = new List<List<CustomVector2Int>>();

    public MapData(int uniqueId)
    {
        this.uniqueId = uniqueId;
    }
}

#endif