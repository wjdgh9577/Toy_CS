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
    [MenuItem("Tools/Map/Extract Tilemap Collider")]
    static void ExtractTilemapCollider()
    {
        Map[] maps = Resources.LoadAll<Map>(Config.MAP_PREFAB_PATH);
        SerializedData<MapData> data = new SerializedData<MapData>();

        using (var writer = File.CreateText($"{Config.COMMON_DATA_PATH}/MapData.json"))
        {
            foreach (Map map in maps)
            {
                MapData mapData = new MapData(map.MapInfo.uniqueId);
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

                        mapData.colliderPaths.Add(newPath);
                    }
                }

                data.datas.Add(mapData);
            }

            writer.WriteLine(JsonConvert.SerializeObject(data));
        }
    }
}

#endif
