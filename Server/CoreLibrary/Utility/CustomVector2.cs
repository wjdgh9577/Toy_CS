using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Utility
{
    public class CustomVector2 : IEquatable<CustomVector2>
    {
        public float x;
        public float y;

        public CustomVector2() : this(0, 0) { }

        public CustomVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(CustomVector2 other)
            => this.x == other.x && this.y == other.y;

        public static bool operator ==(CustomVector2 a, CustomVector2 b)
            => a.Equals(b);

        public static bool operator !=(CustomVector2 a, CustomVector2 b)
            => !a.Equals(b);
    }

    public class CustomVector2Int : IEquatable<CustomVector2>
    {
        public int x;
        public int y;

        public CustomVector2Int() : this(0, 0) { }

        public CustomVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public CustomVector2Int(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        public bool Equals(CustomVector2 other)
            => this.x == other.x && this.y == other.y;

        public static bool operator ==(CustomVector2Int a, CustomVector2Int b)
            => a.Equals(b);

        public static bool operator !=(CustomVector2Int a, CustomVector2Int b)
            => !a.Equals(b);
    }
}
