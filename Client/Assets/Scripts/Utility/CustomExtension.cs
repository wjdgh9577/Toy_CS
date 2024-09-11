using CoreLibrary.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomExtension
{
    public static CustomVector2 ToCustomVector2(this Vector2 vector)
        => new CustomVector2(vector.x, vector.y);

    public static CustomVector2 ToCustomVector2(this Vector2Int vector)
        => new CustomVector2(vector.x, vector.y);

    public static CustomVector2Int ToCustomVector2Int(this Vector2 vector)
        => new CustomVector2Int(vector.x, vector.y);

    public static CustomVector2Int ToCustomVector2Int(this Vector2Int vector)
        => new CustomVector2Int(vector.x, vector.y);

    public static Vector2 ToVector2(this CustomVector2 vector)
        => new Vector2(vector.x, vector.y);

    public static Vector2 ToVector2(this CustomVector2Int vector)
        => new Vector2(vector.x, vector.y);

    public static Vector2Int ToVector2Int(this CustomVector2 vector)
        => new Vector2Int((int)vector.x, (int)vector.y);

    public static Vector2Int ToVector2Int(this CustomVector2Int vector)
        => new Vector2Int(vector.x, vector.y);
}
