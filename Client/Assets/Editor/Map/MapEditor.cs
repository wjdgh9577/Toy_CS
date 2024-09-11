using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using CoreLibrary.Utility;

#if UNITY_EDITOR
using UnityEditor;

public class MapEditor
{
    const string DATA_PATH = "../Common/Map";
    const string PREFAB_PATH = "Prefabs/Map";

    [MenuItem("Tools/Map/Extract Tilemap Collider")]
    static void ExtractTilemapCollider()
    {
        Map[] maps = Resources.LoadAll<Map>(PREFAB_PATH);
        foreach (Map map in maps)
        {
            using (var writer = File.CreateText($"{DATA_PATH}/{map.name}.json"))
            {
                MapInfo mapInfo = new MapInfo(map.uniqueId);
                CompositeCollider2D[] compositeColliders = map.GetComponentsInChildren<CompositeCollider2D>();

                foreach (var compositeCollider in compositeColliders)
                {
                    if (compositeCollider == null)
                        continue;

                    for (int i = 0; i < compositeCollider.pathCount; i++)
                    {
                        List<Vector2> path = new List<Vector2>();
                        compositeCollider.GetPath(i, path);

                        List<CustomVector2Int> newPath = new List<CustomVector2Int>();
                        foreach (var point in path)
                            newPath.Add(point.ToCustomVector2Int());

                        mapInfo.colliderPaths.Add(newPath);
                    }
                }

                writer.Write(JsonConvert.SerializeObject(mapInfo));
            }
        }
    }
}

#endif
