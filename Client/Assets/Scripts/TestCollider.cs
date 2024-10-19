#if UNITY_EDITOR
using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    [SerializeField]
    private CompositeCollider2D compositeCollider;

    GUIStyle guiStyle = new GUIStyle();

    private void Reset()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < compositeCollider.pathCount; i++)
        {
            Vector2[] path = new Vector2[compositeCollider.GetPathPointCount(i)];
            compositeCollider.GetPath(i, path);

            for (int j = 0; j < path.Length; j++)
            {
                Vector2 point = path[j];
                Gizmos.DrawSphere(point, 0.2f);

                // 숫자 라벨 추가
                guiStyle.fontSize = 20;
                guiStyle.normal.textColor = Color.white;
                Handles.Label(point, $"{j}", guiStyle);
            }
        }
    }
}
#endif