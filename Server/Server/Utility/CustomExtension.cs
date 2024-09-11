using CoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utility
{
    public static class CustomExtension
    {
        public static CustomVector2 ToCustomVector2(this Vector2 vector)
            => new CustomVector2(vector.X, vector.Y);

        public static CustomVector2Int ToCustomVector2Int(this Vector2 vector)
            => new CustomVector2Int(vector.X, vector.Y);

        public static Vector2 ToVector2(this CustomVector2 vector)
            => new Vector2(vector.x, vector.y);

        public static Vector2 ToVector2(this CustomVector2Int vector)
            => new Vector2(vector.x, vector.y);
    }
}
