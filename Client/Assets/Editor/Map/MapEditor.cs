using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;

public class MapEditor
{
    const string DATA_PATH = "../Common/Map";

    [MenuItem("Tools/Map/Extract Tilemap Collider")]
    static void ExtractTilemapCollider()
    {
        using (var writer = File.CreateText($"{DATA_PATH}/Map.json"))
        {
            CompositeCollider2D[] compositeColliders = Transform.FindObjectsOfType<CompositeCollider2D>();

            foreach (var compositeCollider in compositeColliders)
            {
                if (compositeCollider == null)
                    continue;

                for (int i = 0; i < compositeCollider.pathCount; i++)
                {
                    Vector2[] path = new Vector2[compositeCollider.GetPathPointCount(i)];
                    compositeCollider.GetPath(i, path);

                    for (int j = 0; j < path.Length; j++)
                    {
                        Vector2 point = path[j];

                        // TODO: Map Data Schema First
                        writer.Write(point);
                    }
                }
            }
        }
    }
}

#endif
