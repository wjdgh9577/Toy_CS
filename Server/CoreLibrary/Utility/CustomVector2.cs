using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Utility
{
    public struct CustomVector2 : IEquatable<CustomVector2>, IEquatable<CustomVector2Int>
    {
        public float x;
        public float y;

        public CustomVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public CustomVector2(CustomVector2Int cv2i)
        {
            this.x = cv2i.x;
            this.y = cv2i.y;
        }

        public bool Equals(CustomVector2 other)
            => this.x == other.x && this.y == other.y;

        public bool Equals(CustomVector2Int other)
            => this.x == other.x && this.y == other.y;

        public static bool operator ==(CustomVector2 a, CustomVector2 b)
            => a.Equals(b);

        public static bool operator ==(CustomVector2 a, CustomVector2Int b)
            => a.Equals(b);

        public static bool operator !=(CustomVector2 a, CustomVector2 b)
            => !a.Equals(b);

        public static bool operator !=(CustomVector2 a, CustomVector2Int b)
            => !a.Equals(b);

        public static CustomVector2 operator +(CustomVector2 a, CustomVector2 b)
            => new CustomVector2(a.x + b.x, a.y + b.y);

        public static CustomVector2 operator +(CustomVector2Int a, CustomVector2 b)
            => new CustomVector2(a.x + b.x, a.y + b.y);

        public static CustomVector2 operator +(CustomVector2 a, CustomVector2Int b)
            => new CustomVector2(a.x + b.x, a.y + b.y);

        public static CustomVector2 operator -(CustomVector2 a, CustomVector2 b)
            => new CustomVector2(a.x - b.x, a.y - b.y);

        public static CustomVector2 operator -(CustomVector2Int a, CustomVector2 b)
            => new CustomVector2(a.x - b.x, a.y - b.y);

        public static CustomVector2 operator -(CustomVector2 a, CustomVector2Int b)
            => new CustomVector2(a.x - b.x, a.y - b.y);

        public CustomVector2Int ToCustomVector2Int()
            => new CustomVector2Int(this);
    }

    public struct CustomVector2Int : IEquatable<CustomVector2Int>, IEquatable<CustomVector2>
    {
        public int x;
        public int y;

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

        public CustomVector2Int(CustomVector2 cv2)
        {
            this.x = (int)cv2.x;
            this.y = (int)cv2.y;
        }

        public bool Equals(CustomVector2Int other)
            => this.x == other.x && this.y == other.y;

        public bool Equals(CustomVector2 other)
            => this.x == other.x && this.y == other.y;

        public static bool operator ==(CustomVector2Int a, CustomVector2Int b)
            => a.Equals(b);

        public static bool operator ==(CustomVector2Int a, CustomVector2 b)
            => a.Equals(b);

        public static bool operator !=(CustomVector2Int a, CustomVector2Int b)
            => !a.Equals(b);

        public static bool operator !=(CustomVector2Int a, CustomVector2 b)
            => !a.Equals(b);

        public static CustomVector2Int operator +(CustomVector2Int a, CustomVector2Int b)
            => new CustomVector2Int(a.x + b.x, a.y + b.y);

        public static CustomVector2Int operator -(CustomVector2Int a, CustomVector2Int b)
            => new CustomVector2Int(a.x - b.x, a.y - b.y);

        public CustomVector2 ToCustomVector2()
            => new CustomVector2(this);
    }
}
