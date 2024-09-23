using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    MapInfo _mapInfo;

    public MapInfo MapInfo { get { return _mapInfo; } }
}
