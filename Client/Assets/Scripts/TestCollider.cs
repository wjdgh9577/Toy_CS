using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    private CompositeCollider2D compositeCollider;

    void Start()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
    }

    void OnDrawGizmos()
    {
        if (compositeCollider == null)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < compositeCollider.pathCount; i++)
        {
            Vector2[] path = new Vector2[compositeCollider.GetPathPointCount(i)];
            compositeCollider.GetPath(i, path);

            for (int j = 0; j < path.Length; j++)
            {
                Vector2 point = path[j];
                Gizmos.DrawSphere(point, 0.1f);

                // 숫자 라벨 추가
                Handles.Label(point, $"{j}");
            }
        }
    }
}
