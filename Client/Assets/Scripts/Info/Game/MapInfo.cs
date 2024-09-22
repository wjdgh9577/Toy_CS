using CoreLibrary.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public int uniqueId;
    public List<List<CustomVector2Int>> colliderPaths = new List<List<CustomVector2Int>>();

    public MapInfo(int uniqueId)
    {
        this.uniqueId = uniqueId;
    }
}
